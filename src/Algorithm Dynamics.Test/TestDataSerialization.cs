using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithm_Dynamics.Core.Helpers;
using System.IO;
using System.Collections.Generic;
using Algorithm_Dynamics.Core.Models;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestDataSerialization
    {
        [TestInitialize]
        public void InitDb()
        {
            string path = $"{Guid.NewGuid()}.db";
            File.Delete(path);
            DataAccess.InitializeDatabase(path);
        }
        [TestMethod]
        public void TestSerializeProblem()
        {
            Problem problem = DatabaseHelper.CreateFullProblem();
            string str = DataSerialization.SerializeProblem(problem);
            
            Assert.AreEqual("Problem", DataSerialization.GetDataType(str));

            Problem result = DataSerialization.DeserializeProblem(str);

            Assert.AreNotEqual(result.Id, problem.Id);
            Assert.AreEqual(result.Uid, problem.Uid);
            Assert.AreEqual(problem.Name, result.Name);
            Assert.AreEqual(problem.Description, result.Description);
            Assert.AreEqual(problem.TimeLimit, result.TimeLimit);
            Assert.AreEqual(problem.MemoryLimit, result.MemoryLimit);
            Assert.AreEqual(problem.Difficulty, result.Difficulty);
            Assert.AreEqual(problem.Tags.Count, result.Tags.Count);
            for (int i = 0; i < problem.Tags.Count; i++)
            {
                Assert.AreEqual(problem.Tags[i].Name, result.Tags[i].Name);
            }
            Assert.AreEqual(problem.TestCases.Count, result.TestCases.Count);
            for (int i = 0; i < problem.TestCases.Count; i++)
            {
                Assert.AreEqual(problem.TestCases[i].Input, result.TestCases[i].Input);
                Assert.AreEqual(problem.TestCases[i].Output, result.TestCases[i].Output);
            }
        }
        [TestMethod]
        public void TestSerializeProblemList()
        {
            ProblemList problemList = DatabaseHelper.CreateFullProblemList();
            string str = DataSerialization.SerializeProblemList(problemList);

            Assert.AreEqual("ProblemList", DataSerialization.GetDataType(str));

            ProblemList result = DataSerialization.DeserializeProblemList(str);
            Assert.AreNotEqual(result.Id, problemList.Id);
            Assert.AreEqual(problemList.Name, result.Name);
            Assert.AreEqual(problemList.Description, result.Description);
            Assert.AreEqual(problemList.Problems.Count, result.Problems.Count);
            for (int i = 0; i < problemList.Problems.Count; i++)
            {
                var expected = problemList.Problems[i];
                var actual = result.Problems[i];
                Assert.AreNotEqual(expected.Id, actual.Id);
                Assert.AreEqual(expected.Uid, actual.Uid);
                Assert.AreEqual(expected.Name, actual.Name);
                Assert.AreEqual(expected.Description, actual.Description);
                Assert.AreEqual(expected.TimeLimit, actual.TimeLimit);
                Assert.AreEqual(expected.MemoryLimit, actual.MemoryLimit);
                Assert.AreEqual(expected.Difficulty, actual.Difficulty);
                Assert.AreEqual(expected.Tags.Count, actual.Tags.Count);
                for (int j = 0; j < expected.Tags.Count; j++)
                {
                    Assert.AreEqual(expected.Tags[j].Name, actual.Tags[j].Name);
                }
                Assert.AreEqual(expected.TestCases.Count, actual.TestCases.Count);
                for (int j = 0; j < expected.TestCases.Count; j++)
                {
                    Assert.AreEqual(expected.TestCases[j].Input, actual.TestCases[j].Input);
                    Assert.AreEqual(expected.TestCases[j].Output, actual.TestCases[j].Output);
                }
            }
        }
    }
}
