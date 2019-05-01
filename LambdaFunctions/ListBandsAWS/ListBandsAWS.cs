using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Noisera.Domain;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ListBandsAWS
{
    public class ListBandsAWS
    {
        public JObject ListBandsHandler()
        {
            string server = Environment.GetEnvironmentVariable("db_server");
            string database = Environment.GetEnvironmentVariable("db_name");
            string username = Environment.GetEnvironmentVariable("db_user");
            string pwd = Environment.GetEnvironmentVariable("db_pass");
            string port = Environment.GetEnvironmentVariable("db_port");
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

            MySqlConnection Conn = new MySqlConnection(ConnectionString);

            JObject response = ListBands(Conn);

            return response;
        }

        private JObject ListBands(MySqlConnection conn)
        {
            var Cmd = new MySqlCommand($"SELECT * FROM bands", conn)
            {
                CommandTimeout = 0
            };

            conn.Open();
            var DataReader = Cmd.ExecuteReader();

            List<Band> BandsList = new List<Band>();

            while (DataReader.Read())
            {
                Band Band = new Band(
                    DataReader["GUID"].ToString(),
                    DataReader["Name"].ToString()
                );
                BandsList.Add(Band);
            }

            JObject Bands = new JObject
            {
                ["Bands"] = JToken.FromObject(BandsList)
            };

            conn.Close();
            return Bands;
        }
    }
}
