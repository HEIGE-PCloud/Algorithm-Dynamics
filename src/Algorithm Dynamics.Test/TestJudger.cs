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
        [TestInitialize]
        public void SetCodeFilePath()
        {
            Judger.SetSourceCodeFilePath(".", "sol");
        }
        [TestMethod]
        public async Task TestRunCodePython()
        {
            string code = "print('hello world')";
            string input = "";
            int timeLimit = 1 * ms;
            int memoryLimit = 64 * MB;
            string expected = "hello world\n";
            RunCodeResult result = await Judger.RunCode(code, input, LanguageConfig.Python, timeLimit, memoryLimit, new Progress<int>());
            //Assert.AreEqual(expected, result.StandardOutput);
        }

        [TestMethod]
        public async Task TestRunCodeCpp()
        {
            string code = "#include <iostream>\nusing namespace std;\nint main(){cout << \"hello world\" << endl;}";
            RunCodeResult result = await Judger.RunCode(code, "", LanguageConfig.Cpp, 1 * ms, 64 * MB, new Progress<int>());
            //Assert.AreEqual("hello world\n", result.StandardOutput);
        }
    }
}
