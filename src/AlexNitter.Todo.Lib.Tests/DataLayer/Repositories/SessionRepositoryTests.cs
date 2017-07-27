using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlexNitter.Todo.Lib.Tests.DataLayer.Repositories
{

    [TestClass]
    public class SessionRepositoryTests
    {

        [TestMethod]
        public void Insert_Find_Update_Test()
        {
            var repo = new SessionRepository();

            var entity = new Session()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = 1,
                Aktiv = true,
                Erstellungsdatum = DateTime.Now
            };

            repo.Insert(entity);

            var selectedEntity = repo.FindById(entity.Id);

            Assert.IsNotNull(selectedEntity);

            selectedEntity.Aktiv = false;

            repo.Update(selectedEntity);
        }
    }
}
