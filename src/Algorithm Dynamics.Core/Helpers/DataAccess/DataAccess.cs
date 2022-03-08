using Microsoft.Data.Sqlite;
using System.IO;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Store the physical loaction of the database.
        /// Use <see cref="InitializeDatabase(string)"/> to initialize the value.
        /// </summary>
        private static string DbPath;

        /// <summary>
        /// Initialize the database at the <see cref="dbPath"/> given.
        /// Execute CREATE TABLE commands.
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

                // Create Language table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Language
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Name TEXT NOT NULL,
                        DisplayName TEXT NOT NULL,
                        NeedCompile INTEGER NOT NULL,
                        CompileCommand TEXT NOT NULL,
                        CompileArguments TEXT NOT NULL,
                        RunCommand TEXT NOT NULL,
                        RunArguments TEXT NOT NULL,
                        FileExtension TEXT NOT NULL
                    );";

                // Create Submission table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS Submission
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Code TEXT NOT NULL,
                        Time TEXT NOT NULL,
                        LanguageId INTEGER NOT NULL,
                        UserUid TEXT NOT NULL,
                        ProblemId INTEGER NOT NULL,
                        AssignmentSubmissionResultUid TEXT,
                        FOREIGN KEY (LanguageId) REFERENCES Language(Id),
                        FOREIGN KEY (UserUid) REFERENCES User(Uid),
                        FOREIGN KEY (ProblemId) REFERENCES Problem(Id),
                        FOREIGN KEY (AssignmentSubmissionResultUid) REFERENCES AssignmentSubmission(Uid)
                    );";

                SqliteCommand createTable = new(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
    }
}
