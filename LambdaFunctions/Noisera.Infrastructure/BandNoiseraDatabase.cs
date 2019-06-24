using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Noisera.Infrastructure;
using System.Collections.Generic;

namespace Noisera.Infrastructure
{
    public class BandNoiseraDatabase
    {
        public static List<Dictionary<string, string>> GetBandsByUserSpotifyId(string spotifyUserId)
        {
            MySqlConnection conn = GenericNoiseraDatabase.GetDatabaseConnection();

            string query = "SELECT bands.* FROM bands " +
                            "INNER JOIN bands_users ON bands_users.BandGUID = bands.GUID " +
                            "WHERE bands_users.SpotifyUserId = '" + spotifyUserId + "' ";

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
                    string parameterValue = "";

                    if (!reader.IsDBNull(index))
                    {
                        parameterValue = reader.GetString(index);
                    }
                    row.Add(parameterName, parameterValue);
                }

                resultSet.Add(row);
            }

            conn.Close();

            return resultSet;
        }
    }
}
