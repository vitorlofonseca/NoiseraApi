using System;
using System.Net.Http;
using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Noisera.Domain;
using Noisera.Infrastructure;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateBandAWS
{
    public class CreateBandAWS
    {
        public void SaveBand(Band band)
        {
            GenericNoiseraDatabase.Insert(new BandDTO(band), "bands");
            SaveBandUsers(band);
        }

        public void SaveBandUsers(Band band)
        {
            foreach(string spotifyUserId in band.SpotifyUsersId)
            {
                BandUserDTO bandUser = new BandUserDTO(spotifyUserId, band.GUID);
                GenericNoiseraDatabase.Insert(bandUser, "bands_users");
            }
        }

        public HttpResponseMessage CreateBandHandler(JObject input)
        {
            Band band = input.ToObject<Band>();
            SaveBand(band);
            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}
