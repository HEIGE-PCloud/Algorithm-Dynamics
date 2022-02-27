using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static class DataAccess
    {
        private static string DbPath;

        /// <summary>
        /// Initialize the database at the <see cref="dbPath"/> given.
        /// </summary>
        /// <param name="dbPath"></param>
        public static void InitializeDatabase(string dbPath)
        {
            DbPath = dbPath;
            if (!File.Exists(dbPath))
            {
                File.CreateText(dbPath).Dispose();
            }
            using (SqliteConnection db = new($"Filename={DbPath}"))
            {
                db.Open();

                string tableCommand = 
                    @"CREATE TABLE IF NOT EXISTS MyTable 
                    (
                        Primary_Key INTEGER PRIMARY KEY,
                        Text_Entry TEXT
                    );";

                // Create User table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS User 
                    (
                        Uid TEXT PRIMARY KEY NOT NULL,
                        Name TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Role INTEGER NOT NULL
                    );";

                // Create Problem table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Problem
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Uid TEXT NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        Status INTEGER NOT NULL,
                        Difficulty INTEGER NOT NULL,
                        TimeLimit INTEGER NOT NULL,
                        MemoryLimit INTEGER NOT NULL
                    );";

                // CREATE Tag table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Tag
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Name TEXT NOT NULL UNIQUE
                    );";

                // Create TagRecord table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS TagRecord
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        ProblemId INTEGER,
                      	TagId INTEGER,
                        FOREIGN KEY (ProblemId) REFERENCES Problem (Id),
                      	FOREIGN KEY (TagId) REFERENCES Tag (Id)
                    );";

                // Create TestCase table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS TestCase
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Input TEXT NOT NULL,
                        Output TEXT NOT NULL,
                        IsExample INTEGER NOT NULL,
                        ProblemId INTEGER,
                        FOREIGN KEY (ProblemId) REFERENCES Problem (Id)
                    );";

                // Create Submission table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Submission
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Code TEXT,
                        Time TEXT NOT NULL,
                        Language TEXT NOT NULL,
                        UserUid TEXT,
                        ProblemId INTEGER,
                        AssignmentSubmissionResultUid TEXT,
                        FOREIGN KEY (UserUid) REFERENCES User(Uid),
                        FOREIGN KEY (ProblemId) REFERENCES Problem(Id),
                        FOREIGN KEY (AssignmentSubmissionResultUid) REFERENCES AssignmentSubmission(Uid)
                    );";

                // Create ProblemList table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS ProblemList
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT NOT NULL
                    );";

                // Create ProblemListRecord table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS ProblemListRecord
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        ProblemId INTEGER NOT NULL,
                        ProblemListId INTEGER NOT NULL,
                        FOREIGN KEY (ProblemId) REFERENCES Problem(Id),
                        FOREIGN KEY (ProblemListId) REFERENCES ProblemList(Id)
                    );";

                // Create Assignment table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Assignment
                    (
                        Uid TEXT PRIMARY KEY NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        DueDate TEXT NOT NULL,
                        Status INTEGER NOT NULL,
                        Type INTEGER NOT NULL,
                        UserUid TEXT,
                        FOREIGN KEY (UserUid) REFERENCES User(Uid)
                    );";

                SqliteCommand createTable = new(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
        internal static void DropDatabase()
        {
            File.Delete(DbPath);
        }
        internal static void AddData(string inputText)
        {
            using SqliteConnection db = new($"Filename={DbPath}");
            db.Open();

            SqliteCommand insertCommand = new();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
            insertCommand.Parameters.AddWithValue("@Entry", inputText);

            insertCommand.ExecuteReader();

            db.Close();
        }

        internal static List<string> GetData()
        {
            List<string> entries = new();

            using (SqliteConnection db = new($"Filename={DbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }

                db.Close();
            }

            return entries;
        }

        internal static void AddUser(User user)
        {
            using SqliteConnection db = new($"Filename={DbPath}");
            db.Open();

            SqliteCommand insertCommand = new();
            insertCommand.Connection = db;

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO User VALUES (@Uid, @Name, @Email, @Role);";
            insertCommand.Parameters.AddWithValue("@Uid", user.Uid.ToString());
            insertCommand.Parameters.AddWithValue("@Name", user.Name);
            insertCommand.Parameters.AddWithValue("@Email", user.Email);
            insertCommand.Parameters.AddWithValue("@Role", user.Role);

            insertCommand.ExecuteReader();

            db.Close();
        }

        public static User GetUser(Guid Uid)
        {
            SqliteConnection db = new($"Filename={DbPath}");
            User user;
            db.Open();

            SqliteCommand selectCommand = new();
            selectCommand.Connection = db;
            selectCommand.CommandText = "SELECT * FROM User WHERE Uid = @Uid";
            selectCommand.Parameters.AddWithValue("@Uid", Uid.ToString());
            SqliteDataReader query = selectCommand.ExecuteReader();

            if (query.Read())
            {
                user = new(Uid, query.GetString(1), query.GetString(2), (Role)query.GetInt32(3));
            }
            else
            {
                user = new(Uid, "", "", Role.Student);
            }
            db.Close();
            return user;
        }
        internal static List<User> GetAllUsers()
        {
            List<User> users = new();

            using (SqliteConnection db = new($"Filename={DbPath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new("SELECT * from User", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    Guid uid = query.GetGuid(0);
                    string name = query.GetString(1);
                    string email = query.GetString(2);
                    Role role = (Role)query.GetInt32(3);
                    users.Add(new User(uid, name, email, role));
                }

                db.Close();
            }

            return users;
        }

        internal static void EditUser(User user, string newName, string newEmail, Role newRole)
        {
            using SqliteConnection db = new($"Filename={DbPath}");
            db.Open();

            SqliteCommand updateCommand = new();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE User SET Name = @newName, Email = @newEmail, Role = @newRole WHERE Uid = @Uid;";
            updateCommand.Parameters.AddWithValue("@newName", newName);
            updateCommand.Parameters.AddWithValue("@newEmail", newEmail);
            updateCommand.Parameters.AddWithValue("@newRole", newRole);
            updateCommand.Parameters.AddWithValue("@Uid", user.Uid.ToString());

            updateCommand.ExecuteNonQuery();

            db.Close();
        }
    }
}
