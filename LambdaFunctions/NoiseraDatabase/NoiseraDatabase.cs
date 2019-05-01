using System;
using System.Reflection;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Noisera.Core;
using Newtonsoft.Json.Linq;

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

        public List<dynamic> Select(JArray criterias, string table)
        {
            List<string> wheres = new List<string>();

            foreach (var criteria in criterias)
            {
                dynamic filterParameters = JObject.Parse(criteria.ToString());

                string comparison = filterParameters["comparison"];
                string valueFilter = filterParameters["value"];
                string columnType = filterParameters["columnType"];
                string columnName = filterParameters["column"];

                if (columnType == "string")
                {
                    valueFilter = "'" + valueFilter + "'";
                }

                wheres.Add(columnName + " " + comparison + " " + valueFilter);

            }

            MySqlConnection conn = this.GetDatabaseConnection();

            string query = "SELECT * FROM " + table + " WHERE " + string.Join(" AND ", wheres) + ";";

            conn.Open();

            MySqlCommand sqlcmd = new MySqlCommand(query, conn);

            MySqlDataReader reader = sqlcmd.ExecuteReader();

            List<dynamic> resultSet = new List<dynamic>();

            while (reader.Read())
            {
                List<dynamic> item = new List<dynamic>();

                for(var i=0; i<reader.FieldCount; i++)
                {
                    item.Add(reader.GetValue(i));
                }

                resultSet.Add(item);
            }

            conn.Close();

            return resultSet;
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
