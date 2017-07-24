using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlexNitter.Todo.Lib.Tests
{
    [TestClass]
    public class TestclassTests
    {
        [TestMethod]
        public void GetDat_Test()
        {
            var temp = new Testclass();
            var data = temp.GetData();

            Assert.IsTrue(!String.IsNullOrEmpty(data));
        }
    }
}
