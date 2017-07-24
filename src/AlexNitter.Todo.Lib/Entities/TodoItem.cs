using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.Entities
{
    /// <summary>
    /// Repräsentiert einen Todo auf einer Liste
    /// </summary>
    public class TodoItem
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Boolean Aktiv { get; set; }
        public TodoListe Liste { get; set; }
        public DateTime Erstellungsdatum { get; set; }
    }
}