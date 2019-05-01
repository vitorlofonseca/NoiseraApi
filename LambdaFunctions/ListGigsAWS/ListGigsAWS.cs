using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
using Noisera.Domain;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListGigsAWS
{
    public class ListGigsAWS
    {
        public JObject ListGigsHandler()
        {
            string server = Environment.GetEnvironmentVariable("db_server");
            string database = Environment.GetEnvironmentVariable("db_name");
            string username = Environment.GetEnvironmentVariable("db_user");
            string pwd = Environment.GetEnvironmentVariable("db_pass");
            string port = Environment.GetEnvironmentVariable("db_port");
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

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
                    DataReader["GUID"].ToString(),
                    DataReader["Name"].ToString(),
                    DataReader["Description"].ToString(),
                    DataReader["AvatarUrl"].ToString(),
                    DataReader["SpotifyPlaylistId"] == DBNull.Value ? null : (int?)DataReader["SpotifyPlaylistId"],
                    DataReader["BandGUID"].ToString()
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
}