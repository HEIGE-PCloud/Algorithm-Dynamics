using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        internal static TestCaseResult AddTestCaseResult(string stdout, string stderr, int exitCode, long time, long memory, ResultCode resultCode, int? submissionResultId)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO TestCaseResult (StandardOutput, StandardError, ExitCode, CPUTime, MemoryUsage, ResultCode, SubmissionResultId) VALUES (@stdout, @stderr, @exitCode, @time, @memory, @resultCode, @submissionResultId);";
                insertCommand.Parameters.AddWithValue("@stdout", stdout);
                insertCommand.Parameters.AddWithValue("@stderr", stderr);
                insertCommand.Parameters.AddWithValue("@exitCode", exitCode);
                insertCommand.Parameters.AddWithValue("@time", time);
                insertCommand.Parameters.AddWithValue("@memory", memory);
                insertCommand.Parameters.AddWithValue("@resultCode", resultCode);
                insertCommand.Parameters.AddWithValue("@submissionResultId", submissionResultId == null ? DBNull.Value : submissionResultId);

                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new(query.GetInt32(0), stdout, stderr, exitCode, time, memory, resultCode);
            }
        }

        internal static List<TestCaseResult> GetTestCaseResults(int submissionResultId)
        {
            List<TestCaseResult> results = new();
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM TestCaseResult WHERE SubmissionResultId = @SubmissionResultId";
                selectCommand.Parameters.AddWithValue("@SubmissionResultId", submissionResultId);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string stdout = query.GetString(1);
                    string stderr = query.GetString(2);
                    int exitCode = query.GetInt32(3);
                    int time = query.GetInt32(4);
                    int memory = query.GetInt32(5);
                    ResultCode resultCode = (ResultCode)query.GetInt32(6);
                    results.Add(new(id, stdout, stderr, exitCode, time, memory, resultCode));
                }
            }
            return results;
        }

        internal static void EditTestCaseResult(int id, int? submissionResultId)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;

                updateCommand.CommandText = "UPDATE TestCaseResult SET SubmissionResultId = @SubmissionResultId WHERE Id = @Id;";
                updateCommand.Parameters.AddWithValue("@SubmissionResultId", submissionResultId == null ? DBNull.Value : submissionResultId);
                updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

    }
}
