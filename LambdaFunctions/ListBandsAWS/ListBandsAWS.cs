using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Amazon.Lambda.Core;
using Noisera.Infrastructure;
using Noisera.Domain;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListBandsAWS
{
    public class ListBandsAWS
    {
        public JObject ListBandsHandler(JObject input)
        {
            List<Band> BandsList = getBandsList(input["spotifyUserId"].ToString());

            JObject Bands = new JObject
            {
                ["bands"] = JToken.FromObject(BandsList)
            };

            JObject response = Bands;

            return response;
        }

        private List<Band> getBandsList(string spotifyUserId)
        {
            List<Dictionary<string, string>> genericBands = BandNoiseraDatabase.GetBandsByUserSpotifyId(spotifyUserId);
            List<Band> bandList = new List<Band>();

            foreach (Dictionary<string, string> genericBand in genericBands)
            {
                List<string> SpotifyUsersId = getSpotifyUsersIdByBandGUID(genericBand["GUID"].ToString());

                Band band = new Band(
                   genericBand["GUID"].ToString(),
                   genericBand["Name"].ToString(),
                   SpotifyUsersId
               );

                bandList.Add(band);

            }

            return bandList;
        }

        private List<string> getSpotifyUsersIdByBandGUID(string bandGuid)
        {
            List<Dictionary<string, string>> bandsUsers = GenericNoiseraDatabase.Select(
                JArray.Parse("[{" +
                    "\"value\": \"" + bandGuid + "\", " +
                    "\"comparison\": \"=\", " +
                    "\"columnType\": \"string\", " +
                    "\"column\": \"BandGUID\"" +
                "}]"), 
                "bands_users");
            List<string> SpotifyUsersIds = new List<string>();

            foreach (Dictionary<string, string> bandUser in bandsUsers)
            {
                SpotifyUsersIds.Add(bandUser["SpotifyUserId"]);
            }

            return SpotifyUsersIds;
        }
    }
}
