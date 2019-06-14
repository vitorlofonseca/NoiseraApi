﻿using System;
using System.Reflection;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Noisera.Core;
using Newtonsoft.Json.Linq;

namespace Noisera.Infrastructure
{
    public class GenericNoiseraDatabase
    {
        public static MySqlConnection GetDatabaseConnection()
        {
            string server = Environment.GetEnvironmentVariable("db_server");
            string database = Environment.GetEnvironmentVariable("db_name");
            string username = Environment.GetEnvironmentVariable("db_user");
            string pwd = Environment.GetEnvironmentVariable("db_pass");
            string port = Environment.GetEnvironmentVariable("db_port");
            string ConnectionString = String.Format("Server={0}; Port={4}; Database={1}; Uid={2}; Pwd={3};", server, database, username, pwd, port);

            return new MySqlConnection(ConnectionString);
        }

        public static List<Dictionary<string, string>> Select(JArray criterias, string table)
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

            MySqlConnection conn = GetDatabaseConnection();

            string query = "SELECT * FROM " + table;

            if (wheres.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", wheres) + ";";
            }

            conn.Open();

            MySqlCommand sqlcmd = new MySqlCommand(query, conn);

            MySqlDataReader reader = sqlcmd.ExecuteReader();

            List<Dictionary<string, string>> resultSet = new List<Dictionary<string, string>>();

            while (reader.Read())
            {
                Dictionary<string, string> row = new Dictionary<string, string>();

                for (int index = 0; index < reader.FieldCount; index++)
                {
                    string parameterName = reader.GetName(index);
                    string parameterValue = reader.GetString(index);
                    row.Add(parameterName, parameterValue);
                }

                resultSet.Add(row);
            }

            conn.Close();

            return resultSet;
        }

        public static string Insert(IAggregateRoot objectToInsert, string table)
        {
            MySqlConnection conn = GetDatabaseConnection();
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
                CommandText = "INSERT INTO " + table + "(`" + string.Join("`, `", inColumns) + "`) " +
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