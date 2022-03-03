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
                        TimeLimit INTEGER NOT NULL,
                        MemoryLimit INTEGER NOT NULL,
                        Status INTEGER NOT NULL,
                        Difficulty INTEGER NOT NULL
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

            using (SqliteConnection connection = new($"Filename={DbPath}"))
            {
                connection.Open();

                SqliteCommand selectCommand = new("SELECT Text_Entry from MyTable", connection);

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

        internal static void AddUser(User user)
        {
            using (SqliteConnection connection = new($"Filename={DbPath}"))
            {
                connection.Open();

                SqliteCommand insertCommand = new();
                insertCommand.Connection = connection;

                insertCommand.CommandText = "INSERT INTO User VALUES (@Uid, @Name, @Email, @Role);";
                insertCommand.Parameters.AddWithValue("@Uid", user.Uid.ToString());
                insertCommand.Parameters.AddWithValue("@Name", user.Name);
                insertCommand.Parameters.AddWithValue("@Email", user.Email);
                insertCommand.Parameters.AddWithValue("@Role", user.Role);

                insertCommand.ExecuteNonQuery();
            }
        }

        internal static User GetUser(Guid Uid)
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

        internal static void EditUser(Guid uid, string newName, string newEmail, Role newRole)
        {
            using SqliteConnection db = new($"Filename={DbPath}");
            db.Open();

            SqliteCommand updateCommand = new();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE User SET Name = @newName, Email = @newEmail, Role = @newRole WHERE Uid = @Uid;";
            updateCommand.Parameters.AddWithValue("@newName", newName);
            updateCommand.Parameters.AddWithValue("@newEmail", newEmail);
            updateCommand.Parameters.AddWithValue("@newRole", newRole);
            updateCommand.Parameters.AddWithValue("@Uid", uid.ToString());

            updateCommand.ExecuteNonQuery();

            db.Close();
        }

        internal static Problem AddProblem(Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases, List<Tag> tags)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO Problem (Uid, Name, Description, TimeLimit, MemoryLimit, Status, Difficulty) VALUES (@Uid, @Name, @Description, @TimeLimit, @MemoryLimit, @Status, @Difficulty);";
                insertCommand.Parameters.AddWithValue("@Uid", uid);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Description", description);
                insertCommand.Parameters.AddWithValue("@TimeLimit", timeLimit);
                insertCommand.Parameters.AddWithValue("@MemoryLimit", memoryLimit);
                insertCommand.Parameters.AddWithValue("@Status", status);
                insertCommand.Parameters.AddWithValue("@Difficulty", difficulty);

                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new Problem(query.GetInt32(0), uid, name, description, timeLimit, memoryLimit, status, difficulty, new List<TestCase>() { }, new List<Tag>() { });
            }
        }

        internal static Problem GetProblem(int Id)
        {
            Problem problem;
            using (var connection = new SqliteConnection($"Filename ={ DbPath }"))
            {
                connection.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = connection;
                selectCommand.CommandText = "SELECT * FROM Problem WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", Id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                if (query.Read())
                {
                    problem = new(query.GetInt32(0), query.GetGuid(1), query.GetString(2), query.GetString(3), query.GetInt32(4), query.GetInt32(5), (ProblemStatus)query.GetInt32(6), (Difficulty)query.GetInt32(7), new List<TestCase> { }, new List<Tag> { });
                }
                else
                {
                    throw new KeyNotFoundException($"The Problem with Id = {Id} is not found in the database.");
                }

            }

            return problem;
        }

        internal static List<Problem> GetAllProblems()
        {
            List<Problem> problems = new();

            using (SqliteConnection connection = new($"Filename={DbPath}"))
            {
                connection.Open();

                SqliteCommand selectCommand = new("SELECT * from Problem", connection);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    problems.Add(new(query.GetInt32(0), query.GetGuid(1), query.GetString(2), query.GetString(3), query.GetInt32(4), query.GetInt32(5), (ProblemStatus)query.GetInt32(6), (Difficulty)query.GetInt32(7), new List<TestCase> { }, new List<Tag> { })); 
                }

            }
            return problems;
        }

        /// <summary>
        /// Pass in a <see cref="TestCase"/> without <see cref="TestCase.Id"/>.
        /// Save the <see cref="TestCase"/> into database and return a <see cref="TestCase"/> with Id.
        /// </summary>
        /// <param name="testCase"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        internal static TestCase AddTestCase(string input, string output, bool isExample, int? problemId = null)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO TestCase (Input, Output, IsExample, ProblemId) VALUES (@Input, @Output, @IsExample, @ProblemId);";
                insertCommand.Parameters.AddWithValue("@Input", input);
                insertCommand.Parameters.AddWithValue("@Output", output);
                insertCommand.Parameters.AddWithValue("@IsExample", isExample);
                insertCommand.Parameters.AddWithValue("@ProblemId", problemId == null ? DBNull.Value : problemId);

                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new TestCase(query.GetInt32(0), input, output, isExample);
            }
        }

        internal static List<TestCase> GetAllTestCases()
        {
            List<TestCase> testCases = new();

            using (SqliteConnection connection = new($"Filename={DbPath}"))
            {
                connection.Open();

                SqliteCommand selectCommand = new("SELECT * from TestCase", connection);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    testCases.Add(new(query.GetInt32(0), query.GetString(1), query.GetString(2), query.GetBoolean(3)));
                }
            }
            return testCases;
        }

        internal static void EditTestCase(int id, string newInput, string newOutput, bool newIsExample, int? newProblemId = null)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;

                updateCommand.CommandText = "UPDATE TestCase SET Input = @newInput, Output = @newOutput, IsExample = @newIsExample, ProblemId = @newProblemId WHERE Id = @Id;";
                updateCommand.Parameters.AddWithValue("@newInput", newInput);
                updateCommand.Parameters.AddWithValue("@newOutput", newOutput);
                updateCommand.Parameters.AddWithValue("@newIsExample", newIsExample);
                updateCommand.Parameters.AddWithValue("@newProblemId", newProblemId == null ? DBNull.Value : newProblemId);
                updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteTestCase(int id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM TestCase WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Pass in a <see cref="Tag"/> without <see cref="Tag.Id"/>.
        /// Save the <see cref="Tag"/> into database and return a <see cref="Tag"/> with Id.
        /// </summary>
        /// <param name="testCase"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        internal static Tag AddTag(string name)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO Tag (Name) VALUES (@Name);";
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new Tag(query.GetInt32(0), name);
            }
        }

        internal static List<Tag> GetAllTags()
        {
            List<Tag> tags = new();

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from Tag", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    tags.Add(new(query.GetInt32(0), query.GetString(1)));
                }
            }
            return tags;
        }
        
        internal static bool TagExists(string name)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT EXISTS (SELECT 1 FROM Tag WHERE Name = @Name)";
                selectCommand.Parameters.AddWithValue("@Name", name);

                SqliteDataReader query = selectCommand.ExecuteReader();
                query.Read();
                return query.GetBoolean(0);
            }
        }

        internal static Tag? GetTag(int id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM Tag WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", id);

                SqliteDataReader query = selectCommand.ExecuteReader();
                if (query.Read())
                {
                    return new Tag(query.GetInt32(0), query.GetString(1));
                }
                else
                {
                    return null;
                }
            }
        }
        internal static Tag GetTag(string name)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM Tag WHERE Name = @Name";
                selectCommand.Parameters.AddWithValue("@Name", name);

                SqliteDataReader query = selectCommand.ExecuteReader();
                if (query.Read())
                {
                    return new Tag(query.GetInt32(0), query.GetString(1));
                }
                else
                {
                    return null;
                }
            }
        }

        internal static void AddTagRecord(int problemId, int tagId)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO TagRecord (ProblemId, TagId) VALUES (@ProblemId, @TagId);";
                insertCommand.Parameters.AddWithValue("@ProblemId", problemId);
                insertCommand.Parameters.AddWithValue("@TagId", tagId);

                insertCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteTagRecord(int problemId, int tagId)
        {
            using (SqliteConnection conn = new($"FileName={DbPath}"))
            {
                conn.Open();
                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM TagRecord WHERE ProblemId = @ProblemId AND TagId = @TagId";
                deleteCommand.Parameters.AddWithValue("@ProblemId", problemId);
                deleteCommand.Parameters.AddWithValue("@TagId", tagId);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
