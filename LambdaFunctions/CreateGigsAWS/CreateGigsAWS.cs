using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
using System.Net.Http;
using Noisera.Domain;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateGigsAWS
{
    public class CreateGigsAWS
    {
        public HttpResponseMessage CreateGigsHandler(JObject input)
        {
            string server = Environment.GetEnvironmentVariable("db_server");
            string database = Environment.GetEnvironmentVariable("db_name");
            string username = Environment.GetEnvironmentVariable("db_user");
            string pwd = Environment.GetEnvironmentVariable("db_pass");
            string port = Environment.GetEnvironmentVariable("db_port");
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

            MySqlConnection Conn = new MySqlConnection(ConnectionString);

            CreateGig(Conn, input.ToObject<Gig>());

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }

        private void CreateGig(MySqlConnection conn, Gig gig)
        {
            var Cmd = new MySqlCommand($"", conn)
            {
                CommandTimeout = 0,
                CommandText = "INSERT INTO gigs(guid, name, description, avatar_url, spotify_playlist_id, band_guid) " +
                    "VALUES(@GUID, @Name, @Description, @AvatarUrl, @SpotifyPlaylistId, @BandGUID);"
            };
            Cmd.Parameters.AddWithValue("@GUID", gig.GUID);
            Cmd.Parameters.AddWithValue("@Name", gig.Name);
            Cmd.Parameters.AddWithValue("@Description", gig.Description);
            Cmd.Parameters.AddWithValue("@AvatarUrl", gig.AvatarUrl);
            Cmd.Parameters.AddWithValue("@SpotifyPlaylistId", gig.SpotifyPlaylistId);
            Cmd.Parameters.AddWithValue("@BandGUID", gig.BandGUID);

            conn.Open();
            Cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}