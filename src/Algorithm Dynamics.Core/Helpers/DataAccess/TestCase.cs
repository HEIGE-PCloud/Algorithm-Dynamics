using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Pass in a <see cref="TestCase"/> without <see cref="TestCase.Id"/>.
        /// Save the <see cref="TestCase"/> into database and return a <see cref="TestCase"/> with Id.
        /// </summary>
        /// <param name="testCase"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        internal static TestCase AddTestCase(string input, string output, bool isExample, int? problemId = null)
        {
            using (SqliteConnection conn = new(ConnectionString))
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

        /// <summary>
        /// Return all test cases in the database as a list
        /// </summary>
        /// <returns></returns>
        internal static List<TestCase> GetAllTestCases()
        {
            List<TestCase> testCases = new();

            using (SqliteConnection connection = new(ConnectionString))
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

        /// <summary>
        /// Edit an existing test case with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newInput"></param>
        /// <param name="newOutput"></param>
        /// <param name="newIsExample"></param>
        /// <param name="newProblemId"></param>
        internal static void EditTestCase(int id, string newInput, string newOutput, bool newIsExample, int? newProblemId = null)
        {
            using (SqliteConnection conn = new(ConnectionString))
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

        /// <summary>
        /// Delete a test case with the given id
        /// </summary>
        /// <param name="id"></param>
        internal static void DeleteTestCase(int id)
        {
            using (SqliteConnection conn = new(ConnectionString))
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
        /// Get all TestCases under a <see cref="Problem"/> by <see cref="Problem.Id"/>.
        /// </summary>
        /// <returns></returns>
        internal static List<TestCase> GetTestCases(int problemId)
        {
            List<TestCase> testCases = new();
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM TestCase WHERE ProblemId = @ProblemId";
                selectCommand.Parameters.AddWithValue("@ProblemId", problemId);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    testCases.Add(new TestCase(query.GetInt32(0), query.GetString(1), query.GetString(2), query.GetBoolean(3)));
                }
            }
            return testCases;
        }
    }
}
