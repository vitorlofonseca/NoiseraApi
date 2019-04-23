using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListGigsAWS
{
    public class ListGigsAWS
    {
        public JObject FunctionHandler()
        {
            string server = "noisera-db.cx3rqev00cim.us-east-1.rds.amazonaws.com";
            string database = "noiseradb";
            string username = "noiserauser";
            string pwd = "noiserapass";
            string ConnectionString = String.Format("Server={0}; Port=3306; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd);

            MySqlConnection Conn = new MySqlConnection(ConnectionString);

            JObject response = ListGigs(Conn);

            return response;
        }

        private JObject ListGigs(MySqlConnection conn)
        {
            var Cmd = new MySqlCommand($"SELECT * FROM gigs", conn)
            {
                CommandTimeout = 0
            };

            conn.Open();
            var DataReader = Cmd.ExecuteReader();

            List<Gig> GigsList = new List<Gig>();

            while (DataReader.Read())
            {
                Gig Gig = new Gig(
                    DataReader["name"].ToString(),
                    DataReader["description"].ToString(),
                    DataReader["avatar_url"].ToString(),
                    DataReader["spotify_playlist_id"] == DBNull.Value ? null : (int?)DataReader["spotify_playlist_id"]
                );
                GigsList.Add(Gig);
            }

            JObject Gigs = new JObject
            {
                ["Gigs"] = JToken.FromObject(GigsList)
            };

            conn.Close();
            return Gigs;
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