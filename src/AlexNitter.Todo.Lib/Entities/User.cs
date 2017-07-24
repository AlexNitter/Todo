using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.Entities
{
    /// <summary>
    /// Repräsentiert einen registrierten Benutzer der Todo-Anwendung
    /// </summary>
    public class User
    {
        public Int32 Id { get; set; }
        public String Username { get; set; }
        public List<TodoListe> Listen { get; set; }
    }
}