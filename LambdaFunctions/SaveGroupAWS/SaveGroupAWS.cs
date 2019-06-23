using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SaveGroupAWS
{
    public class SaveGroupsAWS
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

        public void insertGig(Gig gig)
        {
            GenericNoiseraDatabase.Insert(new GigDTO(gig), "gigs");

            foreach (Track track in gig.Tracks)
            {
                string guidOfStoredTrack = TrackNoiseraDatabase.getGuidOfStoredTrack(track);

                if (guidOfStoredTrack == null)
                {
                    GenericNoiseraDatabase.Insert(new TrackDTO(track), "tracks");
                }
                else
                {
                    track.GUID = guidOfStoredTrack;
                }

                GenericNoiseraDatabase.Insert(new GigTrackDTO(track, gig), "tracks_gig");
            }
        }

        public HttpResponseMessage SaveGroupHandler(JObject input)
        {
            Gig gig = input.ToObject<Gig>();
            gig.Tracks = getTracks(input);

            insertGig(gig);

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}
