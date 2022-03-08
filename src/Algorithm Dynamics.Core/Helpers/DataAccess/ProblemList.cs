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
                return new(query.GetInt32(0), name, description, problems);
            }
        }

        internal static void EditProblemList(int id, string name, string description)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;

                updateCommand.CommandText = "UPDATE ProblemList SET Name = @Name, Description = @Description WHERE Id = @Id;";
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@Description", description);
                updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

        internal static ProblemList GetProblemList(int Id)
        {
            ProblemList problemList;

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * FROM ProblemList WHERE Id = @Id", conn);
                selectCommand.Parameters.AddWithValue("@Id", Id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                if (query.Read())
                {
                    int id = query.GetInt32(0);
                    string name = query.GetString(1);
                    string description = query.GetString(2);
                    problemList = new(id, name, description, GetProblems(id));
                }
                else
                {
                    throw new KeyNotFoundException($"The ProblemList with Id = {Id} is not found in the database.");
                }
            }
            return problemList;
        }

        internal static List<ProblemList> GetAllProblemLists()
        {
            List<ProblemList> problemLists = new();

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from ProblemList", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string name = query.GetString(1);
                    string description = query.GetString(2);
                    problemLists.Add(new(id, name, description, GetProblems(id)));
                }
            }
            return problemLists;
        }

        internal static void AddProblemListRecord(int problemListId, int problemId)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;

                insertCommand.CommandText = "INSERT INTO ProblemListRecord (ProblemId, ProblemListId) VALUES (@ProblemId, @ProblemListId);";
                insertCommand.Parameters.AddWithValue("@ProblemId", problemId);
                insertCommand.Parameters.AddWithValue("@ProblemListId", problemListId);

                insertCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteProblemListRecord(int problemListId, int problemId)
        {
            using (SqliteConnection conn = new($"FileName={DbPath}"))
            {
                conn.Open();
                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM ProblemListRecord WHERE ProblemId = @ProblemId AND ProblemListId = @ProblemListId;";
                deleteCommand.Parameters.AddWithValue("@ProblemId", problemId);
                deleteCommand.Parameters.AddWithValue("@ProblemListId", problemListId);
                deleteCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteProblemList(int id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM ProblemList WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
