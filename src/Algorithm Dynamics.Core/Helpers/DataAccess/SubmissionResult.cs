using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        internal static SubmissionResult AddSubmissionResult(Submission submission, List<TestCaseResult> results)
        {
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO SubmissionResult (SubmissionId) VALUES (@SubmissionId);";
                insertCommand.Parameters.AddWithValue("@SubmissionId", submission.Id);
                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new(query.GetInt32(0), submission, results);
            }
        }


        internal static SubmissionResult GetSubmissionResult(int Id)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM SubmissionResult WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", Id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                if (query.Read())
                {
                    int id = query.GetInt32(0);
                    Submission submission = GetSubmission(query.GetInt32(1));
                    List<TestCaseResult> results = GetTestCaseResults(id);
                    return new(id, submission, results);
                }
                else
                {
                    return null;
                }
            }
        }

        internal static List<SubmissionResult> GetAllSubmissionResults()
        {
            List<SubmissionResult> submissionResults = new();

            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from SubmissionResult", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    Submission submission = GetSubmission(query.GetInt32(1));
                    List<TestCaseResult> results = GetTestCaseResults(id);
                    submissionResults.Add(new(id, submission, results));
                }
            }
            return submissionResults;
        }

        internal static void DeleteSubmissionResult(int id)
        {
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM SubmissionResult WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
