using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.Entities
{
    /// <summary>
    /// Repräsentiert eine Todo-Liste eines Users
    /// </summary>
    public class TodoListe
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public List<TodoItem> Items { get; set; }
        public User Besitzer { get; set; }
        public DateTime Erstellungsdatum { get; set; }
    }
}
