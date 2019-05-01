using System;
using System.Reflection;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Noisera.Core;

namespace Noisera.Infrastructure
{
    public class NoiseraDatabase
    {
        public MySqlConnection GetDatabaseConnection()
        {
            string server = "SERVER";
            string database = "DATABASE";
            string username = "USER";
            string pwd = "PASS";
            string port = "PORT";
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

            return new MySqlConnection(ConnectionString);
        }

        public string Insert(IAggregateRoot objectToInsert, string table)
        {

            MySqlConnection conn = this.GetDatabaseConnection();
            List<string> inColumns = new List<string>();
            List<string> nameVariableToValue = new List<string>();

            List<PropertyInfo> valuesToInsert = new List<PropertyInfo>(objectToInsert.GetType().GetProperties());

            foreach (PropertyInfo column in valuesToInsert)
            {
                var valueToInsert = column.GetValue(objectToInsert, null);
                string columnName = (column.ToString()).Split(" ")[1];

                inColumns.Add(columnName);
                nameVariableToValue.Add("@" + columnName);
            }

            var Cmd = new MySqlCommand($"", conn)
            {
                CommandTimeout = 0,
                CommandText = "INSERT INTO " + table + "(" + string.Join(",", inColumns) + ") " +
                    "VALUES(" + string.Join(",", nameVariableToValue) + ");"
            };

            foreach (PropertyInfo column in valuesToInsert)
            {
                var valueToInsert = column.GetValue(objectToInsert, null);
                string columnName = (column.ToString()).Split(" ")[1];

                Cmd.Parameters.AddWithValue("@" + columnName, valueToInsert);
            }

            conn.Open();
            Cmd.ExecuteNonQuery();
            conn.Close();

            return "";
        }
    }
}
