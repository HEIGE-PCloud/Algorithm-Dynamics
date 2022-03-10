using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        internal static Submission AddSubmission(string code, DateTime time, Language language, User user, Problem problem)
        {

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO Submission (Code, Time, LanguageId, UserUid, ProblemId) VALUES (@Code, @Time, @LanguageId, @UserUid, @ProblemId);";
                insertCommand.Parameters.AddWithValue("@Code", code);
                // https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/types
                insertCommand.Parameters.AddWithValue("@Time", time.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"));
                insertCommand.Parameters.AddWithValue("@LanguageId", language.Id);
                insertCommand.Parameters.AddWithValue("@UserUid", user.Uid.ToString());
                insertCommand.Parameters.AddWithValue("@ProblemId", problem.Id);

                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new Submission(query.GetInt32(0), code, time, language, user, problem);
            }
        }

        internal static Submission GetSubmission(int Id)
        {
            using (var conn = new SqliteConnection($"Filename ={ DbPath }"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM Submission WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", Id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                if (query.Read())
                {
                    int id = query.GetInt32(0);
                    string code = query.GetString(1);
                    DateTime time = query.GetDateTime(2);
                    Language lang = GetLanguage(query.GetInt32(3));
                    User user = GetUser(query.GetGuid(4));
                    Problem problem = GetProblem(query.GetInt32(5));
                    return new(id, code, time, lang, user, problem);
                }
                else
                {
                    return null;
                }
            }
        }

        internal static List<Submission> GetAllSubmissions()
        {
            List<Submission> submissions = new();

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from Submission", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string code = query.GetString(1);
                    DateTime time = query.GetDateTime(2);
                    Language lang = GetLanguage(query.GetInt32(3));
                    User user = GetUser(query.GetGuid(4));
                    Problem problem = GetProblem(query.GetInt32(5));
                    submissions.Add(new(id, code, time, lang, user, problem));
                }
            }
            return submissions;
        }
    }
}
