using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        /// <summary>
        /// Create a new problem in the database
        /// and return that instance
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="timeLimit"></param>
        /// <param name="memoryLimit"></param>
        /// <param name="status"></param>
        /// <param name="difficulty"></param>
        /// <param name="testCases"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        internal static Problem AddProblem(Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases, List<Tag> tags)
        {
            using (SqliteConnection conn = new(ConnectionString))
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
                return new Problem(query.GetInt32(0), uid, name, description, timeLimit, memoryLimit, status, difficulty, testCases, tags);
            }
        }

        /// <summary>
        /// Get the problem with the given id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        internal static Problem GetProblem(int Id)
        {
            Problem problem;
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = connection;
                selectCommand.CommandText = "SELECT * FROM Problem WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", Id);
                SqliteDataReader query = selectCommand.ExecuteReader();

                if (query.Read())
                {
                    int id = query.GetInt32(0);
                    Guid uid = query.GetGuid(1);
                    string name = query.GetString(2);
                    string description = query.GetString(3);
                    int timeLimit = query.GetInt32(4);
                    int memoryLimit = query.GetInt32(5);
                    ProblemStatus status = (ProblemStatus)query.GetInt32(6);
                    Difficulty difficulty = (Difficulty)query.GetInt32(7);
                    problem = new(id, uid, name, description, timeLimit, memoryLimit, status, difficulty, GetTestCases(id), GetTags(id));
                }
                else
                {
                    throw new KeyNotFoundException($"The Problem with Id = {Id} is not found in the database.");
                }
            }
            return problem;
        }

        /// <summary>
        /// Get all Problems under a ProblemList
        /// </summary>
        /// <returns></returns>
        internal static List<Problem> GetProblems(int problemListId)
        {
            List<Problem> problems = new();
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT ProblemListRecord.ProblemId, Problem.Uid, Problem.Name, Problem.Description, Problem.TimeLimit, Problem.MemoryLimit, Problem.Status, Problem.Difficulty FROM ProblemListRecord INNER JOIN Problem ON ProblemListRecord.ProblemId = Problem.Id WHERE ProblemListId = @ProblemListId";
                selectCommand.Parameters.AddWithValue("@ProblemListId", problemListId);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    Guid uid = query.GetGuid(1);
                    string name = query.GetString(2);
                    string description = query.GetString(3);
                    int timeLimit = query.GetInt32(4);
                    int memoryLimit = query.GetInt32(5);
                    ProblemStatus status = (ProblemStatus)query.GetInt32(6);
                    Difficulty difficulty = (Difficulty)query.GetInt32(7);
                    problems.Add(new Problem(id, uid, name, description, timeLimit, memoryLimit, status, difficulty, GetTestCases(id), GetTags(id)));
                }
            }
            return problems;
        }

        /// <summary>
        /// Edit a given problem data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="timeLimit"></param>
        /// <param name="memoryLimit"></param>
        /// <param name="status"></param>
        /// <param name="difficulty"></param>
        internal static void EditProblem(int id, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty)
        {
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();
                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;
                updateCommand.CommandText = "UPDATE Problem SET Name = @Name, Description = @Description, TimeLimit = @TimeLimit, MemoryLimit = @MemoryLimit, Status = @Status, Difficulty = @Difficulty WHERE Id = @Id;";
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@Description", description);
                updateCommand.Parameters.AddWithValue("@TimeLimit", timeLimit);
                updateCommand.Parameters.AddWithValue("@MemoryLimit", memoryLimit);
                updateCommand.Parameters.AddWithValue("@Status", status);
                updateCommand.Parameters.AddWithValue("@Difficulty", difficulty);
                updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Return all problems in the database
        /// </summary>
        /// <returns></returns>
        internal static List<Problem> GetAllProblems()
        {
            List<Problem> problems = new();

            using (SqliteConnection connection = new(ConnectionString))
            {
                connection.Open();

                SqliteCommand selectCommand = new("SELECT * from Problem", connection);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    Guid uid = query.GetGuid(1);
                    string name = query.GetString(2);
                    string description = query.GetString(3);
                    int timeLimit = query.GetInt32(4);
                    int memoryLimit = query.GetInt32(5);
                    ProblemStatus status = (ProblemStatus)query.GetInt32(6);
                    Difficulty difficulty = (Difficulty)query.GetInt32(7);
                    problems.Add(new(id, uid, name, description, timeLimit, memoryLimit, status, difficulty, GetTestCases(id), GetTags(id)));
                }

            }
            return problems;
        }

        /// <summary>
        /// Delete the given problem
        /// </summary>
        /// <param name="id"></param>
        internal static void DeleteProblem(int id)
        {
            using (SqliteConnection conn = new(ConnectionString))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM Problem WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
