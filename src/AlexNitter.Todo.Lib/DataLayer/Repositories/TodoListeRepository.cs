using AlexNitter.Todo.Lib.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AlexNitter.Todo.Lib.DataLayer.Repositories
{
    /// <summary>
    /// Zugriffsschicht auf die TodoListe-Tabelle in der Datenbank
    /// </summary>
    public class TodoListeRepository
    {
        /// <summary>
        /// Liefert die Entity mit der Id oder null, falls keine existieren
        /// </summary>
        /// <param name="userId">Id des Users, dessen TodoListen gefunden werden sollen</param>
        /// <returns></returns>
        public TodoListe FindById(Int32 id)
        {
            TodoListe entity = null;

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM TodoListe WHERE Id = @Id LIMIT 1";
                    command.Parameters.Add(new SqliteParameter("@Id", id));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entity = getEntityFromReader(reader);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Liefert alle Entities mit der UserId oder eine leere Liste, falls keine existieren
        /// </summary>
        /// <param name="userId">Id des Users, dessen TodoListen gefunden werden sollen</param>
        /// <returns></returns>
        public List<TodoListe> FindByUserId(Int32 userId)
        {
            List<TodoListe> entities = new List<TodoListe>();

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM TodoListe WHERE BesitzerId = @BesitzerId";
                    command.Parameters.Add(new SqliteParameter("@BesitzerId", userId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TodoListe entity = getEntityFromReader(reader);

                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// Fügt die übergebene TodoListe in die Datenbank und setzt die Id-Property des Users
        /// </summary>
        public void Insert(TodoListe entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO TodoListe (BesitzerId, Name, Erstellungsdatum) VALUES (@BesitzerId, @Name, @Erstellungsdatum); SELECT last_insert_rowid();";
                    command.Parameters.Add(new SqliteParameter("@BesitzerId", entity.Besitzer.Id));
                    command.Parameters.Add(new SqliteParameter("@Name", entity.Name));
                    command.Parameters.Add(new SqliteParameter("@Erstellungsdatum", Parser.DateTimeToString(entity.Erstellungsdatum)));

                    entity.Id = Int32.Parse(command.ExecuteScalar().ToString());
                }
            }
        }

        /// <summary>
        /// Aktualisiert den Namen des Datenbanksatzes mit der Id der Entity mit dem Namen der Entity
        /// </summary>
        public void Update(TodoListe entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE TodoListe SET Name = @Name";
                    command.Parameters.Add(new SqliteParameter("@Name", entity.Name));

                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Löscht den Datensatz mit der Id
        /// </summary>
        public void Delete(Int32 id)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM TodoListe WHERE Id = @Id";
                    command.Parameters.Add(new SqliteParameter("@Id", id));

                    command.ExecuteNonQuery();
                }
            }
        }

        private TodoListe getEntityFromReader(SqliteDataReader reader)
        {
            return new TodoListe()
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                BesitzerId = Int32.Parse(reader["BesitzerId"].ToString()),
                Name = reader["Name"].ToString(),
                Erstellungsdatum = Parser.StringToDateTime(reader["Erstellungsdatum"].ToString())
            };
        }
    }
}
