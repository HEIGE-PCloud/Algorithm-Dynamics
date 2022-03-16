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
        const string helloWorldPy = "print('hello world')";
        const string helloWorldCpp = "#include <iostream>\nusing namespace std;\nint main(){cout << \"hello world\" << endl;}";
        [TestInitialize]
        public void SetCodeFilePath()
        {
            Judger.SetSourceCodeFilePath(".", "sol");
        }
        [TestMethod]
        public async Task TestRunCodePython()
        {
            string code = helloWorldPy;
            string input = "";
            int timeLimit = 1 * ms;
            int memoryLimit = 64 * MB;
            string expected = "hello world\n";
            RunCodeResult result = await Judger.RunCode(code, input, LanguageConfig.Python, timeLimit, memoryLimit, new Progress<int>());
            Assert.AreEqual(expected, result.StandardOutput);
        }

        [TestMethod]
        public async Task TestRunCodeCpp()
        {
            string code = helloWorldCpp;
            RunCodeResult result = await Judger.RunCode(code, "", LanguageConfig.Cpp, 1 * ms, 64 * MB, new Progress<int>());
            Assert.AreEqual("hello world\n", result.StandardOutput);
        }

        [TestMethod, TestCategory("TestSystemError")]
        public async Task TestCompileSystemError()
        {
            // Set up a non-exsiting compiler g--
            RunCodeResult result = await Judger.RunCode(
                helloWorldCpp,
                "",
                new("C++", "cpp", ".cpp", true, "g--", "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", ""),
                1 * ms,
                64 * MB,
                new Progress<int>()
            );
            // Check the result code and error message
            Assert.AreEqual(result.ResultCode, ResultCode.SYSTEM_ERROR);
            Assert.AreEqual(result.StandardError, $"The CompileCommand g-- cannot be found.\nPlease check the programming language configuration.");
        }

        [TestMethod, TestCategory("TestSystemError")]
        public async Task TestExecuteSystemError()
        {
            // Set up a non-exsiting interpreter p
            RunCodeResult result = await Judger.RunCode(
                helloWorldCpp,
                "",
                new("Python", "python", ".py", "p", "{SourceCodeFilePath}"),
                1 * ms,
                64 * MB,
                new Progress<int>()
            );
            // Check the result code and error message
            Assert.AreEqual(result.ResultCode, ResultCode.SYSTEM_ERROR);
            Assert.AreEqual(result.StandardError, $"The RunCommand p cannot be found.\nPlease check the programming language configuration.");
        }
    }
}
