using AlexNitter.Todo.Lib.Entities;
using Microsoft.Data.Sqlite;
using System;

namespace AlexNitter.Todo.Lib.DataLayer.Repositories
{
    /// <summary>
    /// Zugriffsschicht auf die User-Tabelle in der Datenbank
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Liefert den ersten User mit der übergebenen Id oder null, falls keiner gefunden wird
        /// </summary>
        /// <param name="username">Vollständiger Username, nach dem gesucht werden soll</param>
        public User FindById(Int32 id)
        {
            User entity = null;

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM User WHERE Id = @Id LIMIT 1";
                    command.Parameters.Add(new SqliteParameter("@Id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = getEntityByReader(reader);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Liefert den ersten User mit dem übergebenen Usernamen oder null, falls keiner gefunden wird
        /// </summary>
        /// <param name="username">Vollständiger Username, nach dem gesucht werden soll</param>
        public User FindByUsername(String username)
        {
            User entity = null;

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM User WHERE Username = @Username LIMIT 1";
                    command.Parameters.Add(new SqliteParameter("@Username", username));

                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            entity = getEntityByReader(reader);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Fügt den übergebenen User in die Datenbank und setzt die Id-Property des Users
        /// </summary>
        public void Insert(User entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO User (Username, PasswortHash, PasswortSalt) VALUES (@Username, @PasswortHash, @PasswortSalt); SELECT last_insert_rowid();";
                    command.Parameters.Add(new SqliteParameter("@Username", entity.Username));
                    command.Parameters.Add(new SqliteParameter("@PasswortHash", entity.PasswortHash));
                    command.Parameters.Add(new SqliteParameter("@PasswortSalt", entity.PasswortSalt));

                    entity.Id = Int32.Parse(command.ExecuteScalar().ToString());
                }
            }
        }

        private User getEntityByReader(SqliteDataReader reader)
        {
            return new User()
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                Username = reader["Username"].ToString(),
                PasswortHash = reader["PasswortHash"].ToString(),
                PasswortSalt = reader["PasswortSalt"].ToString()
            };
        }
    }
}
