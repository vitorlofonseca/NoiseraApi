using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Noisera.Domain;
using System.Collections.Generic;

namespace Noisera.Infrastructure
{
    public class TrackNoiseraDatabase
    {
        public static List<Dictionary<string, string>> GetTracksByGigGuid(string gigGuid)
        {
            MySqlConnection conn = GenericNoiseraDatabase.GetDatabaseConnection();

            string query = "SELECT * FROM tracks " +
                            "INNER JOIN tracks_gig ON tracks_gig.TrackGUID = tracks.GUID " +
                            "WHERE tracks_gig.GigGUID = '" + gigGuid + "'";

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

        public static string getGuidOfStoredTrack(Track track)
        {
            JArray filterByGUID = new JArray();
            filterByGUID.Add(JObject.Parse("{" +
                    "\"value\": \"" + track.SpotifyId + "\", " +
                    "\"comparison\": \"=\", " +
                    "\"columnType\": \"string\", " +
                    "\"column\": \"SpotifyTrackId\"" +

                "}"));

            List<Dictionary<string, string>> returnedTracks = GenericNoiseraDatabase.Select(filterByGUID, "tracks");

            foreach (Dictionary<string, string> returnedTrack in returnedTracks)
            {
                return returnedTrack["GUID"];
            }

            return null;
        }
    }
}
