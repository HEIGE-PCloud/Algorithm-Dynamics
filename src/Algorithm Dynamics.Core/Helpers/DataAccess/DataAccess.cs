﻿using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Store the connection config of the database.
        /// Use <see cref="InitializeDatabase(string)"/> to initialize the value.
        /// </summary>
        private static string ConnectionString;

        /// <summary>
        /// Initialize the database at the <see cref="dbPath"/> given.
        /// Execute CREATE TABLE commands.
        /// </summary>
        /// <param name="dbPath"></param>
        public static void InitializeDatabase(string dbPath = "")
        {
            // Create a memory database if dbPath is empty
            if (string.IsNullOrWhiteSpace(dbPath))
            {
                ConnectionString = $"Data Source=InMemoryDb;Mode=Memory;Cache=Shared";
            }
            else
            {
                // Create a new database if not exist
                ConnectionString = $"Data Source={dbPath}";
                if (!File.Exists(dbPath))
                {
                    File.CreateText(dbPath).Dispose();
                }
            }

            // Create tables
            using (SqliteConnection db = new(ConnectionString))
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
                        LanguageId INTEGER,
                        UserUid TEXT NOT NULL,
                        ProblemId INTEGER NOT NULL,
                        FOREIGN KEY (ProblemId) REFERENCES Problem(Id),
                        FOREIGN KEY (LanguageId) REFERENCES Language(Id),
                        FOREIGN KEY (UserUid) REFERENCES User(Uid)
                    );";

                // Create SubmissionResult table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS SubmissionResult
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        SubmissionId INTEGER NOT NULL,
                        FOREIGN KEY (SubmissionId) REFERENCES Submission(Id)
                    );";

                // Create TestCaseResult table
                tableCommand +=
                    @"CREATE TABLE IF NOT EXISTS TestCaseResult
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        StandardOutput TEXT NOT NULL,
                        StandardError TEXT NOT NULL,
                        ExitCode INTEGER NOT NULL,
                        CPUTime INTEGER NOT NULL,
                        MemoryUsage INTEGER NOT NULL,
                        ResultCode INTEGER NOT NULL,
                        SubmissionResultId INTEGER,
                        FOREIGN KEY (SubmissionResultId) REFERENCES SubmissionResult(Id)
                    );";

                SqliteCommand createTable = new(tableCommand, db);

                createTable.ExecuteNonQuery();
            }
        }

        public static void DropDatabase()
        {
            using (SqliteConnection db = new(ConnectionString))
            {
                db.Open();

                string tableCommand =
                    @"DROP TABLE IF EXISTS MyTable;";

                // Drop TestCaseResult table
                tableCommand +=
                    @"DROP TABLE IF EXISTS TestCaseResult;";

                // Drop SubmissionResult table
                tableCommand +=
                    @"DROP TABLE IF EXISTS SubmissionResult;";

                // Drop Submission table
                tableCommand +=
                    @"DROP TABLE IF EXISTS Submission;";

                // Drop Language table
                tableCommand +=
                    @"DROP TABLE IF EXISTS Language;";

                // Drop TestCase table
                tableCommand +=
                    @"DROP TABLE IF EXISTS TestCase;";

                // Drop TagRecord table
                tableCommand +=
                    @"DROP TABLE IF EXISTS TagRecord;";

                // Drop Tag table
                tableCommand +=
                    @"DROP TABLE IF EXISTS Tag;";

                // Drop Problem table
                tableCommand +=
                    @"DROP TABLE IF EXISTS Problem;";

                // Drop User table
                tableCommand +=
                    @"DROP TABLE IF EXISTS User;";

                // Drop ProblemList table
                tableCommand +=
                    @"DROP TABLE IF EXISTS ProblemList;";

                // Drop ProblemListRecord table
                tableCommand +=
                    @"DROP TABLE IF EXISTS ProblemListRecord;";

                SqliteCommand dropTable = new(tableCommand, db);

                dropTable.ExecuteNonQuery();
            }
        }
    }
}
