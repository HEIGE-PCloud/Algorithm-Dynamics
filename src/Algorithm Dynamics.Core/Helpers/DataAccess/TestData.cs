using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Add a text data into the test table in the database.
        /// This procedure is used for testing the function of the database.
        /// </summary>
        /// <param name="inputText"></param>
        internal static void AddData(string inputText)
        {
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

            }
        }

        /// <summary>
        /// Get all data in the test table in the database.
        /// This function is used for testing the function of the database.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetData()
        {
            List<string> entries = new();

            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT Text_Entry from MyTable", conn);

                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(reader.GetString(0));
                    }
                }
            }

            return entries;
        }
    }
}
