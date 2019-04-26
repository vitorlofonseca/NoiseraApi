using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
using System.Net.Http;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateGigsAWS
{
    public class CreateGigsAWS
    {
        public HttpResponseMessage FunctionHandler(JObject input)
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
                CommandText = "INSERT INTO gigs(name, description, avatar_url, spotify_playlist_id) " +
                    "VALUES(@Name, @Description, @AvatarUrl, @SpotifyPlaylistId);"
            };
            Cmd.Parameters.AddWithValue("@Name", gig.Name);
            Cmd.Parameters.AddWithValue("@Description", gig.Description);
            Cmd.Parameters.AddWithValue("@AvatarUrl", gig.AvatarUrl);
            Cmd.Parameters.AddWithValue("@SpotifyPlaylistId", gig.SpotifyPlaylistId);

            conn.Open();
            Cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    class Gig
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AvatarUrl { get; private set; }
        public int? SpotifyPlaylistId { get; private set; }

        public Gig(string name, string description, string avatarUrl, int? spotifyPlaylistId)
        {
            Name = name;
            Description = description;
            AvatarUrl = avatarUrl;
            SpotifyPlaylistId = spotifyPlaylistId;
        }
    }
}