using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AlexNitter.Todo.Lib.DataLayer.Repositories;
using AlexNitter.Todo.Lib.Entities;

namespace AlexNitter.Todo.Lib.Tests.DataLayer.Repositories
{

    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void FindByUsername_Test()
        {
            var username = "Unittest-User";

            var repo = new UserRepository();
            var entity = repo.FindByUsername(username);
        }

        [TestMethod]
        public void FindById_Test()
        {
            var id = 1;

            var repo = new UserRepository();
            var entity = repo.FindById(id);

            Assert.IsNotNull(entity);
        }

        [TestMethod]
        public void Insert_Test()
        {
            var entity = new User()
            {
                Username = "Unittest",
                PasswortHash = "test",
                PasswortSalt = "test"
            };

            var repo = new UserRepository();
            repo.Insert(entity);

            Assert.IsTrue(entity.Id != default(Int32));
        }
    }
}
