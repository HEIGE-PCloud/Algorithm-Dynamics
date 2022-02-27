using Algorithm_Dynamics.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestJudger
    {
        [TestMethod]

        public async Task TestRunCode()
        {
            return;
            string code = "print('hello world')";
            string input = "";
            int timeLimit = 1000;
            int memoryLimit = 64 * 1024 * 1024;
            string expected = "hello world\n";
            Judger.SetSourceCodeFilePath(".", "sol");
            RunCodeResult result = await Judger.RunCode(code, input, LanguageConfig.Python, timeLimit, memoryLimit, new Progress<int>());
            Assert.AreEqual(expected, result.StandardOutput);
        }
    }
}
