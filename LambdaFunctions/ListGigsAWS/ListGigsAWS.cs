using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
using Noisera.Domain;
using Noisera.Infrastructure;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListGigsAWS
{
    public class ListGigsAWS
    {
        public JObject ListGigsHandler()
        {

            List<Gig> GigsList = getGigList();

            JObject Gigs = new JObject
            {
                ["Gigs"] = JToken.FromObject(GigsList)
            };

            JObject response = Gigs;

            return response;
        }

        private List<Track> getTracksByGigGuid(string gigGuid)
        {
            List<Dictionary<string, string>> genericTracks = TrackNoiseraDatabase.GetTracksByGigGuid(gigGuid);
            List<Track> trackList = new List<Track>();

            foreach (Dictionary<string, string> genericTrack in genericTracks)
            {
                Track track = new Track(
                   genericTrack["GUID"],
                   Convert.ToBoolean(Int32.Parse(genericTrack["Active"])),
                   genericTrack["Album"],
                   genericTrack["Artist"],
                   genericTrack["AvatarUrl"],
                   genericTrack["Name"],
                   Int32.Parse(genericTrack["Order"]),
                   genericTrack["SpotifyTrackId"],
                   Int32.Parse(genericTrack["Year"]),
                   genericTrack["Observations"]
                );

                trackList.Add(track);

            }

            return trackList;
        }

        private List<Gig> getGigList()
        {
            List<Dictionary<string, string>> genericGigs = GenericNoiseraDatabase.Select(new JArray(), "gigs");
            List<Gig> gigList = new List<Gig>();

            foreach (Dictionary<string, string> genericGig in genericGigs)
            {
                Gig gig = new Gig(
                   genericGig["GUID"].ToString(),
                   genericGig["Name"].ToString(),
                   genericGig["Description"].ToString(),
                   genericGig["AvatarUrl"].ToString(),
                   genericGig["BandGUID"].ToString()
               );

                gig.Tracks = getTracksByGigGuid(gig.GUID);

                gigList.Add(gig);

            }

            return gigList;
        }
    }
}