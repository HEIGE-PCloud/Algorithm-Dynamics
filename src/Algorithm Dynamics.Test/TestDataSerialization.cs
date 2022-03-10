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
            string fileName = "test.json";

            Problem problem = DatabaseHelper.CreateFullProblem();
            string str = DataSerialization.SerializeProblem(problem);
            File.WriteAllText(fileName, str);


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
    }
}
