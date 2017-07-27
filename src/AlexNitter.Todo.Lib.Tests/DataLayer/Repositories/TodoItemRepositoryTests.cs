using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.Entities;

namespace AlexNitter.Todo.Lib.Tests.DataLayer.Repositories
{

    [TestClass]
    public class TodoItemRepositoryTests
    {
        [TestMethod]
        public void FindById_Test()
        {
            var id = 1;

            var repo = new TodoItemRepository();
            var entity = repo.FindById(id);

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void FindByListId_Test()
        {
            var todoListeId = 1;

            var repo = new TodoItemRepository();
            var entity = repo.FindByTodoListeId(todoListeId);

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void Insert_Test()
        {
            var entity = new TodoItem()
            {
                Name = "Unittest-TodoItem",
                TodoListeId = 1,
                Aktiv = true,
                Erstellungsdatum = DateTime.Now
            };

            var repo = new TodoItemRepository();
            repo.Insert(entity);

            Assert.IsTrue(entity.Id != default(Int32));
        }

        [TestMethod]
        public void Update_Test()
        {
            var repo = new TodoItemRepository();

            var entity = new TodoItem()
            {
                Name = "Unittest-TodoItem",
                TodoListeId = 1,
                Aktiv = true,
                Erstellungsdatum = DateTime.Now
            };

            repo.Insert(entity);
            
            entity.Name = "Unittest-TodoItem UPDATE";

            repo.Update(entity);
        }

        [TestMethod]
        public void Delete_Test()
        {
            var repo = new TodoItemRepository();

            var entity = new TodoItem()
            {
                Name = "Unittest-TodoItem",
                TodoListeId = 1,
                Aktiv = true,
                Erstellungsdatum = DateTime.Now
            };

            repo.Insert(entity);

            repo.Delete(entity.Id);
        }
    }
}
