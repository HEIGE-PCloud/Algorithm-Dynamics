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
        internal static Language AddLanguage(string name, string displayName, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments, string fileExtension)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand insertCommand = new();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO Language (Name, DisplayName, NeedCompile, CompileCommand, CompileArguments, RunCommand, RunArguments, FileExtension) VALUES (@Name, @DisplayName, @NeedCompile, @CompileCommand, @CompileArguments, @RunCommand, @RunArguments, @FileExtension);";
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@DisplayName", displayName);
                insertCommand.Parameters.AddWithValue("@NeedCompile", needCompile);
                insertCommand.Parameters.AddWithValue("@CompileCommand", compileCommand);
                insertCommand.Parameters.AddWithValue("@CompileArguments", compileArguments);
                insertCommand.Parameters.AddWithValue("@RunCommand", runCommand);
                insertCommand.Parameters.AddWithValue("@RunArguments", runArguments);
                insertCommand.Parameters.AddWithValue("@FileExtension", fileExtension);
                insertCommand.ExecuteNonQuery();

                SqliteCommand selectIdCommand = new("SELECT last_insert_rowid();", conn);
                var query = selectIdCommand.ExecuteReader();
                query.Read();
                int id = query.GetInt32(0);
                return new Language(id, name, displayName, needCompile, compileCommand, compileArguments, runCommand, runArguments, fileExtension);
            }
        }

        internal static Language GetLanguage(int Id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();
                SqliteCommand selectCommand = new();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM Language WHERE Id = @Id";
                selectCommand.Parameters.AddWithValue("@Id", Id);

                SqliteDataReader query = selectCommand.ExecuteReader();
                if (query.Read())
                {
                    int id = query.GetInt32(0);
                    string name = query.GetString(1);
                    string displayName = query.GetString(2);
                    bool needCompile = query.GetBoolean(3);
                    string compileCommand = query.GetString(4);
                    string compileArguments = query.GetString(5);
                    string runCommand = query.GetString(6);
                    string runArguments = query.GetString(7);
                    string fileExtension = query.GetString(8);
                    return new Language(id, name, displayName, needCompile, compileCommand, compileArguments, runCommand, runArguments, fileExtension);
                }
                else
                {
                    return null;
                }
            }
        }

        internal static List<Language> GetAllLanguages()
        {
            List<Language> langs = new();

            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand selectCommand = new("SELECT * from Language", conn);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    int id = query.GetInt32(0);
                    string name = query.GetString(1);
                    string displayName = query.GetString(2);
                    bool needCompile = query.GetBoolean(3);
                    string compileCommand = query.GetString(4);
                    string compileArguments = query.GetString(5);
                    string runCommand = query.GetString(6);
                    string runArguments = query.GetString(7);
                    string fileExtension = query.GetString(8);
                    langs.Add(new(id, name, displayName, needCompile, compileCommand, compileArguments, runCommand, runArguments, fileExtension));
                }
            }
            return langs;
        }

        internal static void EditLanguage(int id, string name, string displayName, bool needCompile, string compileCommand, string compileArguments, string runCommand, string runArguments, string fileExtension)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand updateCommand = new();
                updateCommand.Connection = conn;

                updateCommand.CommandText = "UPDATE Language SET Name = @Name, DisplayName = @DisplayName, NeedCompile = @NeedCompile, CompileCommand = @CompileCommand, CompileArguments = @CompileArguments, RunCommand = @RunCommand, RunArguments = @RunArguments, FileExtension = @FileExtension WHERE Id = @Id;";
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@DisplayName", displayName);
                updateCommand.Parameters.AddWithValue("@NeedCompile", needCompile);
                updateCommand.Parameters.AddWithValue("@CompileCommand", compileCommand);
                updateCommand.Parameters.AddWithValue("@CompileArguments", compileArguments);
                updateCommand.Parameters.AddWithValue("@RunCommand", runCommand);
                updateCommand.Parameters.AddWithValue("@RunArguments", runArguments);
                updateCommand.Parameters.AddWithValue("@FileExtension", fileExtension);
                updateCommand.Parameters.AddWithValue("@Id", id);

                updateCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteLanguage(int id)
        {
            using (SqliteConnection conn = new($"Filename={DbPath}"))
            {
                conn.Open();

                SqliteCommand deleteCommand = new();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM Language WHERE Id = @Id";
                deleteCommand.Parameters.AddWithValue("@Id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }
    }
}
