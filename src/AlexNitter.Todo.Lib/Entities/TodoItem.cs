using AlexNitter.Todo.Lib.DataLayer.Repositories;
using Newtonsoft.Json;
using System;

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

        public Int32 TodoListeId { get; set; }

        private TodoListe _liste;

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - sonst würde es zu einer Endlosschleife kommen
        public TodoListe Liste
        {
            get
            {
                if(_liste == null)
                {
                    var repo = new TodoListeRepository();
                    _liste = repo.FindById(TodoListeId);
                }

                return _liste;
            }
        }

        public DateTime Erstellungsdatum { get; set; }
    }
}