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

            List<Dictionary<string, string>> returnedTracks = GenericNoiseraDatabase.Select(filterByGUID, "tracks");

            foreach(Dictionary<string, string> returnedTrack in returnedTracks)
            {
                return returnedTrack["GUID"];
            }

            return null;
        }

        public void insertGig(Gig gig)
        {
            GenericNoiseraDatabase.Insert(new GigDTO(gig), "gigs");

            foreach (Track track in gig.Tracks)
            {
                string guidOfStoredTrack = getGuidOfStoredTrack(track);

                if (guidOfStoredTrack == null)
                {
                    GenericNoiseraDatabase.Insert(new TrackDTO(track), "tracks");
                } else
                {
                    track.GUID = guidOfStoredTrack;
                }
                
                GenericNoiseraDatabase.Insert(new GigTrackDTO(track, gig), "tracks_gig");
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