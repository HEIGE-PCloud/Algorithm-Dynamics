using Algorithm_Dynamics.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestJudger
    {
        const int MB = 1024 * 1024;
        const int ms = 1000;
        [TestMethod]

        public async Task TestRunCode()
        {
            string code = "print('hello world')";
            string input = "";
            int timeLimit = 1 * ms;
            int memoryLimit = 64 * MB;
            string expected = "hello world\n";
            Judger.SetSourceCodeFilePath(".", "sol");
            RunCodeResult result = await Judger.RunCode(code, input, LanguageConfig.Python, timeLimit, memoryLimit, new Progress<int>());
            Assert.AreEqual(expected, result.StandardOutput);
        }
    }
}
