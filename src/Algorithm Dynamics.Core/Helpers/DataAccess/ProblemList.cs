using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Create a new <see cref="ProblemList"/> and save it into the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="problems"></param>
        /// <returns></returns>
        internal static ProblemList AddProblemList(string name, string description, List<Problem> problems)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO ProblemList (Name, Description) VALUES (@Name, @Description);";
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Description", description);
                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                return new ProblemList(query.GetInt32(0), name, description, problems);
            }
        }

        internal static void EditProblemList(string name, string description)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;

                //updateCommand.CommandText = "UPDATE TestCase SET Input = @newInput, Output = @newOutput, IsExample = @newIsExample, ProblemId = @newProblemId WHERE Id = @Id;";
                //updateCommand.Parameters.AddWithValue("@newInput", newInput);
                //updateCommand.Parameters.AddWithValue("@newOutput", newOutput);
                //updateCommand.Parameters.AddWithValue("@newIsExample", newIsExample);
                //updateCommand.Parameters.AddWithValue("@newProblemId", newProblemId == null ? DBNull.Value : newProblemId);
                //updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

        public static List<ProblemList> GetAllProblemLists()
        {
            List<ProblemList> problemLists = new();

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from Tag", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    //problemLists.Add(new(query.GetInt32(0), query.GetString(1), query.GetString(2)));
                }
            }
            return problemLists;
        }
    }
}
