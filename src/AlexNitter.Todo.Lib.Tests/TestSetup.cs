using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.Tests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Config.InitializeConfig(
                connectionstring: @"Data Source=C:\DEV\GitHub\AlexNitter\Todo\src\SolutionItems\todo.db;",
                logFileDirectory: @"C:\Temp\TodoApp\Logs");
        }
    }
}
