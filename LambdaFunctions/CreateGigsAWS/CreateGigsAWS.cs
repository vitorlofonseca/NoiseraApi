using Newtonsoft.Json.Linq;
using Amazon.Lambda.Core;
using Noisera.Domain;
using System.Collections.Generic;
using Noisera.Infrastructure;
using System.Net.Http;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateGigsAWS
{
    public class CreateGigsAWS
    {
        private List<Track> getTracks(JObject requestGig)
        {
            dynamic obj = requestGig;
            List<Track> tracks = new List<Track>();
            foreach (dynamic trackJson in obj.Tracks)
            {
                Track track = trackJson.ToObject<Track>();
                tracks.Add(track);
            }
            return tracks;
        }

        public string getGuidOfStoredTrack(Track track)
        {
            JArray filterByGUID = new JArray();
            filterByGUID.Add(JObject.Parse("{" +
                    "\"value\": \""+ track.SpotifyId + "\", " +
                    "\"comparison\": \"=\", " +
                    "\"columnType\": \"string\", " +
                    "\"column\": \"SpotifyTrackId\"" +

                "}"));

            List<object> returnedTracks = NoiseraDatabase.Select(filterByGUID, "tracks");

            foreach(object returnedTrack in returnedTracks)
            {
                string guid = (string)((List<object>)returnedTrack)[0];
                return guid;
            }

            return null;
        }

        public void insertGig(Gig gig)
        {
            NoiseraDatabase.Insert(new GigDTO(gig), "gigs");

            foreach (Track track in gig.Tracks)
            {
                string guidOfStoredTrack = getGuidOfStoredTrack(track);

                if (guidOfStoredTrack == null)
                {
                    NoiseraDatabase.Insert(new TrackDTO(track), "tracks");
                } else
                {
                    track.GUID = guidOfStoredTrack;
                }
                
                NoiseraDatabase.Insert(new GigTrackDTO(track, gig), "tracks_gig");
            }
        }

        public HttpResponseMessage CreateGigsHandler(JObject input)
        {
            Gig gig = input.ToObject<Gig>();
            gig.Tracks = getTracks(input);

            insertGig(gig);

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}