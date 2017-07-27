using AlexNitter.Todo.Lib.DataLayer.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlexNitter.Todo.Lib.Entities
{
    /// <summary>
    /// Repräsentiert eine Todo-Liste eines Users
    /// </summary>
    public class TodoListe
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        private List<TodoItem> _items;

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - sonst würde es zu einer Endlosschleife kommen
        public List<TodoItem> Items
        {
            get
            {
                if (_items == null)
                {
                    var repo = new TodoItemRepository();
                    _items = repo.FindByTodoListeId(Id).OrderByDescending(x => x.Aktiv).ThenBy(x => x.Name).ToList();
                }

                return _items;
            }
        }

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - sonst würde es zu einer Endlosschleife kommen
        public Int32 BesitzerId { get; set; }

        private User _besitzer;

        [JsonIgnore] // Gibt an, dass die Eigenschaft ignoriert werden soll beim Zurückgeben eines Objekts diesen Typs von einem MVC-Controller - sonst würde es zu einer Endlosschleife kommen
        public User Besitzer
        {
            get
            {
                if (_besitzer == null)
                {
                    var repo = new UserRepository();
                    _besitzer = repo.FindById(BesitzerId);
                }

                return _besitzer;
            }
        }

        public DateTime Erstellungsdatum { get; set; }
    }
}
