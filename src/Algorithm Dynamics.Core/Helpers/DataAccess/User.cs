using Algorithm_Dynamics.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static partial class DataAccess
    {
        internal static void AddUser(User user)
        {
            using (SqliteConnection connection = new(ConnectionString))
            {
                connection.Open();

                SqliteCommand insertCommand = new();
                insertCommand.Connection = connection;

                insertCommand.CommandText = "INSERT INTO User VALUES (@Uid, @Name, @Email, @Role);";
                insertCommand.Parameters.AddWithValue("@Uid", user.Uid.ToString());
                insertCommand.Parameters.AddWithValue("@Name", user.Name);
                insertCommand.Parameters.AddWithValue("@Email", user.Email);
                insertCommand.Parameters.AddWithValue("@Role", user.Role);

                insertCommand.ExecuteNonQuery();
            }
        }

        internal static User GetUser(Guid Uid)
        {
            SqliteConnection db = new(ConnectionString);
            User user;
            db.Open();

            SqliteCommand selectCommand = new();
            selectCommand.Connection = db;
            selectCommand.CommandText = "SELECT * FROM User WHERE Uid = @Uid";
            selectCommand.Parameters.AddWithValue("@Uid", Uid.ToString());
            SqliteDataReader query = selectCommand.ExecuteReader();

            if (query.Read())
            {
                user = new(Uid, query.GetString(1), query.GetString(2), (Role)query.GetInt32(3));
            }
            else
            {
                user = null;
            }
            db.Close();
            return user;
        }
        public static List<User> GetAllUsers()
        {
            List<User> users = new();

            using (SqliteConnection db = new(ConnectionString))
            {
                db.Open();

                SqliteCommand selectCommand = new("SELECT * from User", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    Guid uid = query.GetGuid(0);
                    string name = query.GetString(1);
                    string email = query.GetString(2);
                    Role role = (Role)query.GetInt32(3);
                    users.Add(new User(uid, name, email, role));
                }

                db.Close();
            }

            return users;
        }

        internal static void EditUser(Guid uid, string newName, string newEmail, Role newRole)
        {
            using SqliteConnection db = new(ConnectionString);
            db.Open();

            SqliteCommand updateCommand = new();
            updateCommand.Connection = db;

            updateCommand.CommandText = "UPDATE User SET Name = @newName, Email = @newEmail, Role = @newRole WHERE Uid = @Uid;";
            updateCommand.Parameters.AddWithValue("@newName", newName);
            updateCommand.Parameters.AddWithValue("@newEmail", newEmail);
            updateCommand.Parameters.AddWithValue("@newRole", newRole);
            updateCommand.Parameters.AddWithValue("@Uid", uid.ToString());

            updateCommand.ExecuteNonQuery();

            db.Close();
        }
    }
}
