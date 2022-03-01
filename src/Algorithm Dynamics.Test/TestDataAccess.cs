using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithm_Dynamics.Core.Helpers;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using Algorithm_Dynamics.Core.Models;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestDataAccess
    {
        private void DropDatabase(string dbPath)
        {
            File.Delete(dbPath);
        }
        [TestMethod]
        public void TestSingleData()
        {
            DropDatabase("SingleData.db");
            DataAccess.InitializeDatabase("SingleData.db");
            DataAccess.AddData("Text1");
            Assert.AreEqual(DataAccess.GetData().Count, 1);
            Assert.AreEqual(DataAccess.GetData()[0], "Text1");
        }

        [TestMethod]
        public void TestMultipleData()
        {
            DropDatabase("MultipleData.db");
            DataAccess.InitializeDatabase("MultipleData.db");
            int count = 100;
            List<string> list = new();
            for (int i = 0; i < count; i++)
            {
                DataAccess.AddData($"Text{count}");
                list.Add($"Text{count}");
            }
            Assert.AreEqual(DataAccess.GetData().Count, count);
            CollectionAssert.AreEqual(list, DataAccess.GetData());
        }

        [TestMethod]
        public void TestUserData()
        {
            DropDatabase("UserData.db");
            DataAccess.InitializeDatabase("UserData.db");
            User user = User.Create("Test User", "test@example.com", Role.Student);

            List<User> expectedUsers = new() { user };
            List<User> actualUsers = DataAccess.GetAllUsers();
            Assert.AreEqual(1, actualUsers.Count);
            CollectionAssert.AreEqual(actualUsers, expectedUsers);
        }

        [TestMethod]
        public void TestEditData()
        {
            DropDatabase("EditData.db");
            DataAccess.InitializeDatabase("EditData.db");
            User user = User.Create("Test User", "test@example.com", Role.Student);
            user.Name = "Edited Name";
            user.Email = "new@example.com";
            user.Role = Role.Teacher;

            User actualUser = DataAccess.GetAllUsers()[0];
            Assert.AreEqual(user, actualUser);
        }

        [TestMethod]
        public void TestGetUser()
        {
            DropDatabase("GetUser.db");
            DataAccess.InitializeDatabase("GetUser.db");
            User user = User.Create("Test User", "Test@example.com", Role.Student);
            User actualUser = DataAccess.GetUser(user.Uid);
            Assert.AreEqual(user, actualUser);
        }

        [TestMethod]
        public void TestCreateProblem()
        {
            DropDatabase("AddProblem.db");
            DataAccess.InitializeDatabase("AddProblem.db");
            Problem problem = new(1, Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * 1024 * 1024, ProblemStatus.Todo, Difficulty.Easy, new List<TestCase> { }, new List<Tag> { });
            DataAccess.AddProblem(problem);

            Assert.AreEqual(problem, DataAccess.GetProblem(problem.Id));
        }

        [TestMethod]
        public void TestGetAllProblems()
        {
            DropDatabase("GetAllProblems.db");
            DataAccess.InitializeDatabase("GetAllProblems.db");
            Problem problem = new(1, Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * 1024 * 1024, ProblemStatus.Todo, Difficulty.Easy, new List<TestCase> { }, new List<Tag> { });
            DataAccess.AddProblem(problem);
            Assert.AreEqual(1, DataAccess.GetAllProblems().Count);
            Assert.AreEqual(problem, DataAccess.GetAllProblems()[0]);
        }

        [TestMethod]
        public void TestAddTestCase()
        {
            DropDatabase("AddTestCase.db");
            DataAccess.InitializeDatabase("AddTestCase.db");
            TestCase testCase1 = TestCase.Create("input", "output", true);
            Assert.AreEqual(testCase1, DataAccess.GetAllTestCases()[0]);
        }
    }
}
