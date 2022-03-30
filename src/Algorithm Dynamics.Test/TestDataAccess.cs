using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestDataAccess
    {
        const int MB = 1024 * 1024;

        /// <summary>
        /// Create a temp database with unique name for testing
        /// </summary>
        [TestInitialize]
        public void InitDb()
        {
            string path = $"{Guid.NewGuid()}.db";
            File.Delete(path);
            DataAccess.InitializeDatabase(path);
        }

        /// <summary>
        /// Clean up all temp databases after testing
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            foreach (string path in Directory.GetFiles(".", "*.db"))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Insert a single data into the test table
        /// </summary>
        [TestMethod]
        public void TestSingleData()
        {
            DataAccess.AddData("Text1");
            Assert.AreEqual(DataAccess.GetData().Count, 1);
            Assert.AreEqual(DataAccess.GetData()[0], "Text1");
        }

        [TestMethod]
        public void TestMultipleData()
        {
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
            User user = User.Create("Test User", "test@example.com", Role.Student);

            List<User> expectedUsers = new() { user };
            List<User> actualUsers = DataAccess.GetAllUsers();
            Assert.AreEqual(1, actualUsers.Count);
            CollectionAssert.AreEqual(actualUsers, expectedUsers);
        }

        [TestMethod]
        public void TestEditData()
        {
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
            User user = User.Create("Test User", "Test@example.com", Role.Student);
            User actualUser = DataAccess.GetUser(user.Uid);
            Assert.AreEqual(user, actualUser);
        }

        [TestMethod]
        public void TestCreateProblem()
        {
            TestCase testCase1 = TestCase.Create("input1", "output1", true);
            TestCase testCase2 = TestCase.Create("input2", "output2", false);

            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            var testCases = new List<TestCase>() { testCase1, testCase2 };
            var tags = new List<Tag>() { tag1, tag2 };
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, Difficulty.Easy, testCases, tags);
            Assert.AreEqual(problem, DataAccess.GetProblem(problem.Id));
        }

        [TestMethod]
        public void TestEditProblem()
        {
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, Difficulty.Easy);
            problem.Name = "New name";
            problem.Description = "New description";
            problem.TimeLimit = 2000;
            problem.MemoryLimit = 128 * MB;
            problem.Difficulty = Difficulty.Easy;
            problem.Status = ProblemStatus.Solved;
            Assert.AreEqual(problem, DataAccess.GetProblem(problem.Id));
        }

        [TestMethod]
        public void TestGetAllProblems()
        {
            TestCase testCase1 = TestCase.Create("input1", "output1", true);
            TestCase testCase2 = TestCase.Create("input2", "output2", false);

            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            var testCases1 = new List<TestCase>() { testCase1 };
            var testCases2 = new List<TestCase>() { testCase2 };
            var tags = new List<Tag>() { tag1, tag2 };
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, Difficulty.Easy, testCases1, tags);
            Problem problem2 = Problem.Create(Guid.NewGuid(), "Test Problem2", "Description2", 2000, 6 * MB, Difficulty.Hard, testCases2, tags);
            CollectionAssert.AreEqual(new List<Problem>() { problem, problem2 }, DataAccess.GetAllProblems());
        }

        [TestMethod]
        public void TestAddTestCase()
        {
            TestCase testCase1 = TestCase.Create("input", "output", true);
            Assert.AreEqual(testCase1, DataAccess.GetAllTestCases()[0]);
        }

        [TestMethod]
        public void TestEditTestCase()
        {
            TestCase testCase = TestCase.Create("input", "output", true);
            testCase.Input = "newInput";
            testCase.Output = "newOutput";
            testCase.IsExample = false;
            Assert.AreEqual(testCase, DataAccess.GetAllTestCases()[0]);
        }

        [TestMethod]
        public void TestDeleteTestCase()
        {
            TestCase testCase = TestCase.Create("input", "output", true);
            testCase.Delete();
            Assert.AreEqual(0, DataAccess.GetAllTestCases().Count);
        }

        [TestMethod]
        public void TestAddTag()
        {
            Tag tag = Tag.Create("tag");
            Assert.AreEqual(tag, DataAccess.GetAllTags()[0]);
        }

        [TestMethod]
        public void TestTagExists()
        {
            Assert.AreEqual(false, DataAccess.TagExists("tag"));
            Tag.Create("tag");
            Assert.AreEqual(true, DataAccess.TagExists("tag"));
        }

        [TestMethod]
        public void TestGetTag()
        {
            Tag tag = Tag.Create("tag");
            Assert.AreEqual(tag, DataAccess.GetTag(tag.Id));
            Assert.AreEqual(tag, DataAccess.GetTag(tag.Name));
            Assert.AreEqual(null, DataAccess.GetTag("Non-exist Tag"));
            Assert.AreEqual(null, DataAccess.GetTag(100));
        }

        [TestMethod]
        public void TestAddDeleteTagRecord()
        {
            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            Problem problem = Problem.Create(Guid.NewGuid(), "Test Problem", "Description", 1000, 64 * MB, Difficulty.Easy);
            problem.AddTag(tag1);
            problem.AddTag(tag2);
            problem.RemoveTag(tag1);
            Assert.AreEqual(problem, DataAccess.GetAllProblems()[0]);
        }
        [TestMethod]
        public void TestTagRecordExists()
        {
            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            Problem problem = DatabaseHelper.CreateNewProblem();
            problem.AddTag(tag1);
            Assert.AreEqual(true, DataAccess.TagRecordExists(tag1.Id));
            Assert.AreEqual(false, DataAccess.TagRecordExists(tag2.Id));
            problem.RemoveTag(tag1);
            Assert.AreEqual(false, DataAccess.TagRecordExists(tag1.Id));
            Assert.AreEqual(false, DataAccess.TagExists(tag1.Name));
        }
        [TestMethod]
        public void TestDeleteProblem()
        {
            Tag tag1 = Tag.Create("tag1");
            Tag tag2 = Tag.Create("tag2");
            TestCase testCase1 = TestCase.Create("input1", "output1", true);
            TestCase testCase2 = TestCase.Create("input2", "output2", false);
            Problem problem = DatabaseHelper.CreateNewProblem();
            problem.AddTestCase(testCase1);
            problem.AddTestCase(testCase2);
            problem.AddTag(tag1);
            problem.AddTag(tag2);
            problem.Delete();

            Assert.AreEqual(false, DataAccess.TagExists(tag1.Name));
            Assert.AreEqual(false, DataAccess.TagExists(tag2.Name));
            Assert.AreEqual(false, DataAccess.TagRecordExists(tag1.Id));
            Assert.AreEqual(false, DataAccess.TagRecordExists(tag2.Id));
            Assert.AreEqual(0, DataAccess.GetAllTestCases().Count);
            Assert.AreEqual(0, DataAccess.GetAllProblems().Count);
        }

        [TestMethod]
        public void TestAddProblemList()
        {
            ProblemList problemList = DataAccess.AddProblemList("Problem List", "Description", new());
            Assert.AreEqual(problemList, DataAccess.GetAllProblemLists()[0]);
        }

        [TestMethod]
        public void TestCreateProblemList()
        {
            TestCase testCase1 = DatabaseHelper.CreateNewTestCase();
            TestCase testCase2 = DatabaseHelper.CreateNewTestCase();
            Tag tag1 = DatabaseHelper.CreateNewTag();
            Tag tag2 = DatabaseHelper.CreateNewTag();
            Problem problem1 = DatabaseHelper.CreateNewProblem();
            problem1.AddTestCase(testCase1);
            problem1.AddTag(tag1);
            problem1.AddTag(tag2);
            Problem problem2 = DatabaseHelper.CreateNewProblem();
            problem2.AddTestCase(testCase2);
            ProblemList problemList = ProblemList.Create("Problem List", "Description", new() { problem1, problem2 });
            Assert.AreEqual(problemList, DataAccess.GetAllProblemLists()[0]);
        }

        [TestMethod]
        public void TestEditProblemList()
        {
            ProblemList problemList = ProblemList.Create("Problem List", "Description", new());
            problemList.Name = "New name";
            problemList.Description = "New description";
            Assert.AreEqual(problemList, DataAccess.GetAllProblemLists()[0]);
        }

        [TestMethod]
        public void TestDeleteProblemList()
        {
            ProblemList problemList = ProblemList.Create("Problem List", "Description", new());
            Assert.AreEqual(1, ProblemList.All.Count);
            problemList.Delete();
            Assert.AreEqual(0, ProblemList.All.Count);
        }

        [TestMethod]
        public void TestGetProblemList()
        {
            ProblemList problemList = ProblemList.Create("Problem List", "Description", new());
            Assert.AreEqual(problemList, DataAccess.GetProblemList(problemList.Id));
        }

        [TestMethod]
        public void TestAddDeleteProblemListRecord()
        {
            var problemList = DatabaseHelper.CreateNewProblemList();
            var problem1 = DatabaseHelper.CreateNewProblem();
            var problem2 = DatabaseHelper.CreateNewProblem();
            problemList.AddProblem(problem1);
            problemList.AddProblem(problem2);
            problemList.RemoveProblem(problem1);
            CollectionAssert.AreEqual(problemList.Problems, ProblemList.All[0].Problems);
        }

        [TestMethod]
        public void TestAddLanguage()
        {
            Language language = DataAccess.AddLanguage("name", "displayname", true, "compleCommand", "compileArguments", "runCommand", "runArguments", "fileExtension");
            Assert.AreEqual(language, DataAccess.GetLanguage(language.Id));
        }

        [TestMethod]
        public void TestEditLanguage()
        {
            Language lang = Language.Create("name", "displayname", true, "compileCommand", "compileArguments", "runCommand", "runArguments", "fileExtension");
            lang.Name = "New Name";
            lang.DisplayName = "New DisplayName";
            lang.NeedCompile = false;
            lang.CompileCommand = "New command";
            lang.CompileArguments = "New arguments";
            lang.RunCommand = "New command";
            lang.RunArguments = "New arguments";
            lang.FileExtension = "new extension";
            Assert.AreEqual(lang, DataAccess.GetLanguage(lang.Id));
        }

        [TestMethod]
        public void TestGetAllLanguages()
        {
            Language lang1 = Language.Create("name", "displayname", true, "compileCommand", "compileArguments", "runCommand", "runArguments", "fileExtension");
            Language lang2 = Language.Create("name", "displayname", true, "compileCommand", "compileArguments", "runCommand", "runArguments", "fileExtension");
            CollectionAssert.AreEqual(new List<Language>() { lang1, lang2 }, DataAccess.GetAllLanguages());
        }

        [TestMethod]
        public void TestAddSubmission()
        {
            var problem = DatabaseHelper.CreateNewProblem();
            var user = DatabaseHelper.CreateNewUser();
            var lang = DatabaseHelper.CreateNewLanguage();
            var sub = Submission.Create("code", lang, user, problem);

            Assert.AreEqual(sub, DataAccess.GetSubmission(sub.Id));
        }

        [TestMethod]
        public void TestGetAllSubmissions()
        {
            var problem = DatabaseHelper.CreateNewProblem();
            var user = DatabaseHelper.CreateNewUser();
            var lang = DatabaseHelper.CreateNewLanguage();
            var sub1 = Submission.Create("code", lang, user, problem);
            var sub2 = Submission.Create("code", lang, user, problem);
            Assert.AreEqual(2, DataAccess.GetAllSubmissions().Count);
            CollectionAssert.AreEqual(new List<Submission>() { sub1, sub2 }, DataAccess.GetAllSubmissions());
        }

        [TestMethod]
        public void TestAddSubmissionResult()
        {
            var problem = DatabaseHelper.CreateNewProblem();
            var user = DatabaseHelper.CreateNewUser();
            var lang = DatabaseHelper.CreateNewLanguage();
            var submission = Submission.Create("code", lang, user, problem);

            SubmissionResult submissionResult = SubmissionResult.Create(submission, new());
            Assert.AreEqual(submissionResult, DataAccess.GetSubmissionResult(submissionResult.Id));
        }

        [TestMethod]
        public void TestTestCaseResult()
        {
            var problem = DatabaseHelper.CreateNewProblem();
            var user = DatabaseHelper.CreateNewUser();
            var lang = DatabaseHelper.CreateNewLanguage();
            var submission = Submission.Create("code", lang, user, problem);

            TestCaseResult t1 = TestCaseResult.Create(new("stdout1", "stderr1", 0, 1000, 64 * MB, ResultCode.SUCCESS));
            TestCaseResult t2 = TestCaseResult.Create(new("stdout2", "stderr2", 1, 2000, 16 * MB, ResultCode.TIME_LIMIT_EXCEEDED));
            TestCaseResult t3 = TestCaseResult.Create(new("stdout2", "stderr2", 1, 2000, 16 * MB, ResultCode.MEMORY_LIMIT_EXCEEDED));
            TestCaseResult t4 = TestCaseResult.Create(new("stdout2", "stderr2", 1, 2000, 16 * MB, ResultCode.WRONG_ANSWER));
            TestCaseResult t5 = TestCaseResult.Create(new("stdout2", "stderr2", 1, 2000, 16 * MB, ResultCode.COMPILE_ERROR));
            SubmissionResult result = SubmissionResult.Create(submission, new());
            result.AddTestCaseResult(t1);
            result.AddTestCaseResult(t2);
            result.AddTestCaseResult(t3);
            result.AddTestCaseResult(t4);
            result.AddTestCaseResult(t5);

            Assert.AreEqual(result, DataAccess.GetSubmissionResult(result.Id));
            CollectionAssert.AreEqual(result.Results, DataAccess.GetSubmissionResult(result.Id).Results);
            CollectionAssert.AreEqual(result.Results, DataAccess.GetTestCaseResults(result.Id));
        }

        [TestMethod]
        public void TestDeleteTestCaseResult()
        {
            SubmissionResult result = DatabaseHelper.CreateNewSubmissionResult();
            var testCaseResult = DatabaseHelper.CreateNewTestCaseResult();
            result.AddTestCaseResult(testCaseResult);
            result.RemoveTestCaseResult(testCaseResult);
            Assert.AreEqual(0, DataAccess.GetAllSubmissionResults()[0].Results.Count);
        }

        [TestMethod]
        public void TestDeleteSubmissionResult()
        {
            SubmissionResult result = DatabaseHelper.CreateNewSubmissionResult();
            var t1 = DatabaseHelper.CreateNewTestCaseResult();
            var t2 = DatabaseHelper.CreateNewTestCaseResult();
            var t3 = DatabaseHelper.CreateNewTestCaseResult();
            var t4 = DatabaseHelper.CreateNewTestCaseResult();
            var t5 = DatabaseHelper.CreateNewTestCaseResult();
            result.AddTestCaseResult(t1);
            result.AddTestCaseResult(t2);
            result.AddTestCaseResult(t3);
            result.AddTestCaseResult(t4);
            result.AddTestCaseResult(t5);
            result.Delete();
            Assert.AreEqual(0, DataAccess.GetAllSubmissionResults().Count);
        }

        [TestMethod]
        public void TestDeleteSubmission()
        {
            Submission submission = DatabaseHelper.CreateNewSubmission();
            SubmissionResult result = SubmissionResult.Create(submission, new());
            result.AddTestCaseResult(DatabaseHelper.CreateNewTestCaseResult());
            result.AddTestCaseResult(DatabaseHelper.CreateNewTestCaseResult());
            result.AddTestCaseResult(DatabaseHelper.CreateNewTestCaseResult());
            result.AddTestCaseResult(DatabaseHelper.CreateNewTestCaseResult());
            result.AddTestCaseResult(DatabaseHelper.CreateNewTestCaseResult());
            Assert.AreEqual(1, DataAccess.GetAllSubmissions().Count);
            Assert.AreEqual(1, DataAccess.GetAllSubmissionResults().Count);
            submission.Delete();
            Assert.AreEqual(0, DataAccess.GetAllSubmissions().Count);
            Assert.AreEqual(0, DataAccess.GetAllSubmissionResults().Count);
        }
    }
}
