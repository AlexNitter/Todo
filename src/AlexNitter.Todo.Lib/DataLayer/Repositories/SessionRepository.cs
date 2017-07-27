using AlexNitter.Todo.Lib.Entities;
using Microsoft.Data.Sqlite;
using System;

namespace AlexNitter.Todo.Lib.DataLayer.Repositories
{
    /// <summary>
    /// Zugriffsschicht auf die Session-Tabelle in der Datenbank
    /// </summary>
    public class SessionRepository
    {
        /// <summary>
        /// Liefert die erste Session mit der übergebenen Id oder null, falls keine gefunden wird
        /// </summary>
        /// <param name="username">Vollständiger Username, nach dem gesucht werden soll</param>
        public Session FindById(String id)
        {
            Session entity = null;

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Session WHERE Id = @Id LIMIT 1";
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
        /// Fügt den übergebenen User in die Datenbank und setzt die Id-Property des Users
        /// </summary>
        public void Insert(Session entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Session (Id, UserId, Aktiv, Erstellungsdatum) VALUES (@Id, @UserId, @Aktiv, @Erstellungsdatum)";
                    command.Parameters.Add(new SqliteParameter("@Id", entity.Id));
                    command.Parameters.Add(new SqliteParameter("@UserId", entity.UserId));
                    command.Parameters.Add(new SqliteParameter("@Aktiv", Parser.BooleanToInt(entity.Aktiv)));
                    command.Parameters.Add(new SqliteParameter("@Erstellungsdatum", Parser.DateTimeToString(entity.Erstellungsdatum)));

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Session entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Session SET Aktiv = @Aktiv WHERE Id = @Id";
                    command.Parameters.Add(new SqliteParameter("@Id", entity.Id));
                    command.Parameters.Add(new SqliteParameter("@Aktiv", Parser.BooleanToInt(entity.Aktiv)));

                    command.ExecuteNonQuery();
                }
            }
        }

        private Session getEntityByReader(SqliteDataReader reader)
        {
            return new Session()
            {
                Id = reader["Id"].ToString(),
                UserId = Int32.Parse(reader["UserId"].ToString()),
                Aktiv = Parser.IntToBoolean(Int32.Parse(reader["Aktiv"].ToString())),
                Erstellungsdatum = Parser.StringToDateTime(reader["Erstellungsdatum"].ToString())
            };
        }
    }
}
