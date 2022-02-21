using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static class DataAccess
    {
        private static string DbPath;
        public static void InitializeDatabase(string dbPath)
        {
            DbPath = dbPath;
            File.CreateText(dbPath).Close();
            using (SqliteConnection db = new SqliteConnection($"Filename={DbPath}"))
            {
                db.Open();

                string tableCommand = 
                    @"CREATE TABLE IF NOT EXISTS
                    MyTable (
                        Primary_Key INTEGER PRIMARY KEY,
                        Text_Entry TEXT
                    )";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
                db.Close();
            }
        }

        public static void AddData(string inputText)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={DbPath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static List<string> GetData()
        {
            List<string> entries = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={DbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }

                db.Close();
            }

            return entries;
        }


    }
}
