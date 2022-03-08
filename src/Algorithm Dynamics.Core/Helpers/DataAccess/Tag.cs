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

        /// <summary>
        /// Delete a Tag from the Tag table. You need to check whether there exists a tag record first.
        /// </summary>
        /// <param name="id"></param>
        internal static void DeleteTag(int id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM Tag WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get all tags associate to a problem.
        /// </summary>
        /// <param name="problemId"></param>
        /// <returns></returns>
        internal static List<Tag> GetTags(int problemId)
        {
            List<Tag> tags = new();
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT TagRecord.TagId, Tag.Name FROM TagRecord INNER JOIN Tag ON TagRecord.TagId = Tag.Id WHERE ProblemId = @ProblemId ";
                selectCommand.Parameters.AddWithValue("@ProblemId", problemId);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    tags.Add(new Tag(query.GetInt32(0), query.GetString(1)));
                }
            }
            return tags;
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

        /// <summary>
        /// Check whether there is a tag record for a given tagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        internal static bool TagRecordExists(int tagId)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT EXISTS (SELECT 1 FROM TagRecord WHERE TagId = @TagId)";
                selectCommand.Parameters.AddWithValue("@TagId", tagId);

                SqliteDataReader query = selectCommand.ExecuteReader();
                query.Read();
                return query.GetBoolean(0);
            }
        }
    }
}
