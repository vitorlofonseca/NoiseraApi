using System;
using System.Net.Http;
using Amazon.Lambda.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Noisera.Domain;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CreateBandsAWS
{
    public class CreateBandsAWS
    {
        public HttpResponseMessage CreateBandsHandler(JObject input)
        {
            string server = Environment.GetEnvironmentVariable("db_server");
            string database = Environment.GetEnvironmentVariable("db_name");
            string username = Environment.GetEnvironmentVariable("db_user");
            string pwd = Environment.GetEnvironmentVariable("db_pass");
            string port = Environment.GetEnvironmentVariable("db_port");
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

            MySqlConnection Conn = new MySqlConnection(ConnectionString);

            CreateBand(Conn, input.ToObject<Band>());

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }

        private void CreateBand(MySqlConnection conn, Band band)
        {
            var Cmd = new MySqlCommand($"", conn)
            {
                CommandTimeout = 0,
                CommandText = "INSERT INTO bands(GUID, Name) " +
                    "VALUES(@GUID, @Name);"
            };
            Cmd.Parameters.AddWithValue("@GUID", band.GUID);
            Cmd.Parameters.AddWithValue("@Name", band.Name);

            conn.Open();
            Cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
