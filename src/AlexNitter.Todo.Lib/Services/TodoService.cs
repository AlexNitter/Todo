using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlexNitter.Todo.Lib.Services
{
    public class TodoService
    {
        private UserRepository _userRepo = new UserRepository();
        private TodoListeRepository _listeRepo = new TodoListeRepository();
        private TodoItemRepository _itemRepo = new TodoItemRepository();

        public List<TodoListe> GetListenFromUser(String username)
        {
            var listen = new List<TodoListe>();

            var user = _userRepo.FindByUsername(username);

            if (user != null)
            {
                listen = _listeRepo.FindByUserId(user.Id);
            }

            return listen.OrderBy(x => x.Name).ToList();
        }

        public void CreateNewListe(TodoListe liste, String username)
        {
            var user = _userRepo.FindByUsername(username);

            if (user != null)
            {
                liste.BesitzerId = user.Id;
                liste.Erstellungsdatum = DateTime.Now;

                _listeRepo.Insert(liste);
            }
            else
            {
                throw new Exception("User nicht gefunden");
            }
        }

        public void EditListe(TodoListe liste)
        {
            _listeRepo.Update(liste);
        }

        public void DeleteListe(Int32 listeId)
        {
            _listeRepo.Delete(listeId);
        }

        public void CreateNewItem(TodoItem item)
        {
            item.Aktiv = true;
            item.Erstellungsdatum = DateTime.Now;

            _itemRepo.Insert(item);
        }

        public void EditItem(TodoItem item)
        {
            _itemRepo.Update(item);
        }

        public void ItemErledigt(Int32 itemId)
        {
            var item = _itemRepo.FindById(itemId);
            item.Aktiv = false;

            _itemRepo.Update(item);
        }
    }
}
