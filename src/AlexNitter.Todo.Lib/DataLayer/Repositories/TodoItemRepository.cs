using AlexNitter.Todo.Lib.Entities;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace AlexNitter.Todo.Lib.DataLayer.Repositories
{
    /// <summary>
    /// Zugriffsschicht auf die TodoItem-Tabelle in der Datenbank
    /// </summary>
    public class TodoItemRepository
    {
        /// <summary>
        /// Liefert die Entity mit der Id oder null, falls keine existieren
        /// </summary>
        /// <param name="userId">Id des Users, dessen TodoListen gefunden werden sollen</param>
        /// <returns></returns>
        public TodoItem FindById(Int32 id)
        {
            TodoItem entity = null;

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM TodoItem WHERE Id = @Id LIMIT 1";
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
        public List<TodoItem> FindByTodoListeId(Int32 todoListeId)
        {
            List<TodoItem> entities = new List<TodoItem>();

            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM TodoItem WHERE TodoListId = @TodoListId";
                    command.Parameters.Add(new SqliteParameter("@TodoListId", todoListeId));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TodoItem entity = getEntityFromReader(reader);

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
        public void Insert(TodoItem entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO TodoItem (TodoListId, Name, Aktiv, Erstellungsdatum) VALUES (@TodoListId, @Name, @Aktiv, @Erstellungsdatum); SELECT last_insert_rowid();";
                    command.Parameters.Add(new SqliteParameter("@TodoListId", entity.TodoListeId));
                    command.Parameters.Add(new SqliteParameter("@Name", entity.Name));
                    command.Parameters.Add(new SqliteParameter("@Aktiv", Parser.BooleanToInt(entity.Aktiv)));
                    command.Parameters.Add(new SqliteParameter("@Erstellungsdatum", entity.Erstellungsdatum.ToString(Config.DATE_FORMAT)));

                    entity.Id = Int32.Parse(command.ExecuteScalar().ToString());
                }
            }
        }

        /// <summary>
        /// Aktualisiert den Namen des Datenbanksatzes mit der Id der Entity mit dem Namen der Entity
        /// </summary>
        public void Update(TodoItem entity)
        {
            using (var connection = new SqliteConnection(Config.Connectionstring))
            {
                connection.Open();

                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE TodoItem SET Name = @Name, Aktiv = @Aktiv WHERE Id = @Id";
                    command.Parameters.Add(new SqliteParameter("@Id", entity.Id));
                    command.Parameters.Add(new SqliteParameter("@Name", entity.Name));
                    command.Parameters.Add(new SqliteParameter("@Aktiv", Parser.BooleanToInt(entity.Aktiv)));

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
                    command.CommandText = "DELETE FROM TodoItem WHERE Id = @Id";
                    command.Parameters.Add(new SqliteParameter("@Id", id));

                    command.ExecuteNonQuery();
                }
            }
        }

        private TodoItem getEntityFromReader(SqliteDataReader reader)
        {
            return new TodoItem()
            {
                Id = Int32.Parse(reader["Id"].ToString()),
                TodoListeId = Int32.Parse(reader["TodoListId"].ToString()),
                Name = reader["Name"].ToString(),
                Aktiv = Parser.IntToBoolean(Int32.Parse(reader["Aktiv"].ToString())),
                Erstellungsdatum = Parser.StringToDateTime(reader["Erstellungsdatum"].ToString())
            };
        }
    }
}
