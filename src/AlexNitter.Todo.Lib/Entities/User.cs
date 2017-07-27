using Newtonsoft.Json;
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

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - der PasswortHash darf niemals rausgegeben werden
        public String PasswortHash { get; set; }

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - der PasswortSalt darf niemals rausgegeben werden
        public String PasswortSalt { get; set; }

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - sonst würde es zu einer Endlosschleife kommen
        public List<TodoListe> Listen { get; set; }
    }
}