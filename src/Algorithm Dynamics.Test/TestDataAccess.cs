﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        const int MB = 1024 * 1024;
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
            TestCase testCase1 = TestCase.Create("input1", "output1", true);
            TestCase testCase2 = TestCase.Create("input2", "output2", false);

            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            var testCases = new List<TestCase>() { testCase1, testCase2 };
            var tags = new List<Tag>() { tag1, tag2 };
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, ProblemStatus.Todo, Difficulty.Easy, testCases, tags);
            Assert.AreEqual(problem, DataAccess.GetProblem(problem.Id));
        }

        [TestMethod]
        public void TestGetAllProblems()
        {
            DropDatabase("GetAllProblems.db");
            DataAccess.InitializeDatabase("GetAllProblems.db");
            TestCase testCase1 = TestCase.Create("input1", "output1", true);
            TestCase testCase2 = TestCase.Create("input2", "output2", false);

            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            var testCases = new List<TestCase>() { testCase1, testCase2 };
            var tags = new List<Tag>() { tag1, tag2 };
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, ProblemStatus.Todo, Difficulty.Easy, testCases, tags);
            Problem problem2 = Problem.Create(Guid.NewGuid(), "Test Problem2", "Description2", 2000, 6 * MB, ProblemStatus.Attempted, Difficulty.Hard, testCases, tags);
            CollectionAssert.AreEqual(new List<Problem>() { problem, problem2 }, DataAccess.GetAllProblems());
        }

        [TestMethod]
        public void TestAddTestCase()
        {
            DropDatabase("AddTestCase.db");
            DataAccess.InitializeDatabase("AddTestCase.db");
            TestCase testCase1 = TestCase.Create("input", "output", true);
            Assert.AreEqual(testCase1, DataAccess.GetAllTestCases()[0]);
        }

        [TestMethod]
        public void TestEditTestCase()
        {
            DropDatabase("EditTestCase.db");
            DataAccess.InitializeDatabase("EditTestCase.db");
            TestCase testCase = TestCase.Create("input", "output", true);
            testCase.Input = "newInput";
            testCase.Output = "newOutput";
            testCase.IsExample = false;
            Assert.AreEqual(testCase, DataAccess.GetAllTestCases()[0]);
        }

        [TestMethod]
        public void TestDeleteTestCase()
        {
            DropDatabase("DeleteTestCase.db");
            DataAccess.InitializeDatabase("DeleteTestCase.db");
            TestCase testCase = TestCase.Create("input", "output", true);
            testCase.Delete();
            Assert.AreEqual(0, DataAccess.GetAllTestCases().Count);
        }

        [TestMethod]
        public void TestAddTag()
        {
            DropDatabase("AddTag.db");
            DataAccess.InitializeDatabase("AddTag.db");
            Tag tag = Tag.Create("tag");
            Assert.AreEqual(tag, DataAccess.GetAllTags()[0]);
        }

        [TestMethod]
        public void TestTagExists()
        {
            DropDatabase("TagExists.db");
            DataAccess.InitializeDatabase("TagExists.db");
            Assert.AreEqual(false, DataAccess.TagExists("tag"));
            Tag.Create("tag");
            Assert.AreEqual(true, DataAccess.TagExists("tag"));
        }

        [TestMethod]
        public void TestGetTag()
        {
            DropDatabase("GetTag.db");
            DataAccess.InitializeDatabase("GetTag.db");
            Tag tag = Tag.Create("tag");
            Assert.AreEqual(tag, DataAccess.GetTag(tag.Id));
            Assert.AreEqual(tag, DataAccess.GetTag(tag.Name));
            Assert.AreEqual(null, DataAccess.GetTag("Non-exist Tag"));
            Assert.AreEqual(null, DataAccess.GetTag(100));
        }

        [TestMethod]
        public void TestAddDeleteTagRecord()
        {
            DropDatabase("AddDeleteTagRecord");
            DataAccess.InitializeDatabase("AddDeleteTagRecord");
            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, ProblemStatus.Todo, Difficulty.Easy);
            problem.AddTag(tag1);
            problem.AddTag(tag2);
            problem.RemoveTag(tag1);
            Assert.AreEqual(problem, DataAccess.GetAllProblems()[0]);
        }
    }
}