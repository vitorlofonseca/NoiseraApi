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
        public JObject FunctionHandler(JObject input, ILambdaContext context)
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

        private JObject ListGigs(MySqlConnection DBConnection)
        {
            var Cmd = new MySqlCommand($"SELECT * FROM gigs", DBConnection)
            {
                CommandTimeout = 0
            };

            DBConnection.Open();
            var DataReader = Cmd.ExecuteReader();

            List<Gig> GigsList = new List<Gig>();

            while (DataReader.Read())
            {
                Gig Gig = new Gig(
                    DataReader["name"].ToString(),
                    DataReader["description"].ToString(),
                    DataReader["avatar_url"].ToString(),
                    (int)DataReader["spotify_playlist_id"]
                );
                GigsList.Add(Gig);
            }

            JObject Gigs = new JObject
            {
                ["gigs"] = JToken.FromObject(GigsList)
            };

            return Gigs;
        }
    }

    class Gig
    {
        public string name { get; private set; }
        public string description { get; private set; }
        public string avatar_url { get; private set; }
        public int spotify_playlist_id { get; private set; }

        public Gig(string _name, string _description, string _avatar_url, int _spotify_playlist_id)
        {
            name = _name;
            description = _description;
            avatar_url = _avatar_url;
            spotify_playlist_id = _spotify_playlist_id;
        }
    }
}