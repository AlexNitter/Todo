using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.Entities;

namespace AlexNitter.Todo.Lib.Tests.DataLayer.Repositories
{

    [TestClass]
    public class TodoListeRepositoryTests
    {
        [TestMethod]
        public void FindById_Test()
        {
            var id = 1;

            var repo = new TodoListeRepository();
            var entity = repo.FindById(id);

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void FindByUserId_Test()
        {
            var userId = 1;

            var repo = new TodoListeRepository();
            var entity = repo.FindByUserId(userId);

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void Insert_Test()
        {
            var entity = new TodoListe()
            {
                Name = "Unittest-TodoListe",
                BesitzerId = 1,
                Erstellungsdatum = DateTime.Now
            };

            var repo = new TodoListeRepository();
            repo.Insert(entity);

            Assert.IsTrue(entity.Id != default(Int32));
        }

        [TestMethod]
        public void Update_Test()
        {
            var repo = new TodoListeRepository();

            var entity = new TodoListe()
            {
                Name = "Unittest-TodoListe",
                BesitzerId = 1,
                Erstellungsdatum = DateTime.Now
            };

            repo.Insert(entity);
            
            entity.Name = "Unittest-TodoListe UPDATE";

            repo.Update(entity);
        }

        [TestMethod]
        public void Delete_Test()
        {
            var repo = new TodoListeRepository();

            var entity = new TodoListe()
            {
                Name = "Unittest-TodoListe",
                BesitzerId = 1,
                Erstellungsdatum = DateTime.Now
            };

            repo.Insert(entity);

            repo.Delete(entity.Id);
        }
    }
}
