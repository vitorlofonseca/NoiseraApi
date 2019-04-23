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
            var Cmd = new MySqlCommand($"SELECT * FROM gigs", DBConnection);
            Cmd.CommandTimeout = 0;
            DBConnection.Open();
            var rdr = Cmd.ExecuteReader();
            List<string> gigsList = new List<string>();
            while (rdr.Read())
            {
                gigsList.Add('{' +
                    "name: " + rdr[1].ToString() + ", " +
                    "description: " + rdr[2].ToString() + ", " +
                    "avatar: " + rdr[3].ToString() + " " +
                '}');
            }

            JObject gigs = new JObject();

            gigs["gigs"] = JToken.FromObject(gigsList);

            return gigs;
        }
    }
}