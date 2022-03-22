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
        const int S = 1000;
        const string helloWorldPy = "print('hello world')";
        const string helloWorldCpp = "#include <iostream>\nusing namespace std;\nint main(){cout << \"hello world\" << endl;}";
        const string APlusBPy = "a=int(input())\nb=int(input())\nprint(a+b)";
        const string APlusBCpp = "#include <iostream>\nusing namespace std;\nint main(){int a,b;cin>>a>>b;cout<<a+b;}";
        const string TLEPy = "while True:\n    pass";
        const string TLECpp = "int main(){while(1){}}";
        const string MLEPy = "a=[1]\nwhile True:\n    a+=a";
        const string MLECpp = "const int N = 100000000;int a[N];int main(){for(int i = 0;i<N;i++)a[i]= i;}";
        const string WAPy = "a=int(input())\nb=int(input())\nprint(a+b+1)";
        const string WACpp = "#include <iostream>\nusing namespace std;\nint main(){int a,b;cin>>a>>b;cout<<a+b+1;}";
        const string REPy = "re";
        const string CECpp = "ce";
        
        [TestInitialize]
        public void SetCodeFilePath()
        {
            Judger.SetSourceCodeFilePath(".", "sol");
        }

        /// <summary>
        /// Test running a simple hello world Python program
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRunCode")]
        public async Task TestRunCodePython()
        {
            RunCodeResult result = await Judger.RunCode(helloWorldPy, "", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("hello world\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test running a simple hello world C++ program
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRunCode")]
        public async Task TestRunCodeCpp()
        {
            RunCodeResult result = await Judger.RunCode(helloWorldCpp, "", LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("hello world\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test input processing
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRunCodeInput")]
        public async Task TestRunCodeInputPy()
        {
            RunCodeResult result = await Judger.RunCode(APlusBPy, "3\n4", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("7\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test input processing
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRunCodeInput")]
        public async Task TestRunCodeInputCpp()
        {
            RunCodeResult result = await Judger.RunCode(APlusBCpp, "3\n4", LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("7\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test time limit exceed
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestTLE")]
        public async Task TestTLEPy()
        {
            RunCodeResult result = await Judger.RunCode(TLEPy, "", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime >= 1 * S);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.TIME_LIMIT_EXCEEDED, result.ResultCode);
        }

        /// <summary>
        /// Test time limit exceed
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestTLE")]
        public async Task TestTLECpp()
        {
            RunCodeResult result = await Judger.RunCode(TLECpp, "", LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime >= 1 * S);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.TIME_LIMIT_EXCEEDED, result.ResultCode);
        }

        /// <summary>
        /// Test memory limit exceed
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestMLE")]
        public async Task TestMLEPy()
        {
            RunCodeResult result = await Judger.RunCode(MLEPy, "", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage > 64 * MB);
            Assert.AreEqual(ResultCode.MEMORY_LIMIT_EXCEEDED, result.ResultCode);
        }

        /// <summary>
        /// Test memory limit exceed
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestMLE")]
        public async Task TestMLECpp()
        {
            RunCodeResult result = await Judger.RunCode(MLECpp, "", LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage > 64 * MB);
            Assert.AreEqual(ResultCode.MEMORY_LIMIT_EXCEEDED, result.ResultCode);
        }

        /// <summary>
        /// Test compile error
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestCE")]
        public async Task TestCE()
        {
            RunCodeResult result = await Judger.RunCode(CECpp, "", LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreNotEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.AreEqual(0, result.CPUTime);
            Assert.AreEqual(0, result.MemoryUsage);
            Assert.AreEqual(ResultCode.COMPILE_ERROR, result.ResultCode);
        }

        /// <summary>
        /// Test runtime error
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRE")]
        public async Task TestRE()
        {
            RunCodeResult result = await Judger.RunCode(REPy, "", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardOutput);
            Assert.AreNotEqual("", result.StandardError);
            Assert.AreNotEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime > 0 && result.CPUTime < 2 * S);
            Assert.IsTrue(result.MemoryUsage > 0 && result.MemoryUsage < 64 * MB);
            Assert.AreEqual(ResultCode.RUNTIME_ERROR, result.ResultCode);
        }

        /// <summary>
        /// Test incorrect compiler config
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestSystemError")]
        public async Task TestCompileSystemError()
        {
            // Set up a non-exsiting compiler g--
            RunCodeResult result = await Judger.RunCode(
                helloWorldCpp,
                "",
                new("C++", "cpp", ".cpp", true, "g--", "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", ""),
                1 * S,
                64 * MB,
                new Progress<int>()
            );
            // Check the result code and error message
            Assert.AreEqual(result.ResultCode, ResultCode.SYSTEM_ERROR);
            Assert.AreEqual(result.StandardError, $"The CompileCommand g-- cannot be found.\nPlease check the programming language configuration.");
        }
        
        /// <summary>
        /// Test incorrect run command config
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestSystemError")]
        public async Task TestExecuteSystemError()
        {
            // Set up a non-exsiting interpreter p
            RunCodeResult result = await Judger.RunCode(
                helloWorldCpp,
                "",
                new("Python", "python", ".py", "p", "{SourceCodeFilePath}"),
                1 * S,
                64 * MB,
                new Progress<int>()
            );
            // Check the result code and error message
            Assert.AreEqual(result.ResultCode, ResultCode.SYSTEM_ERROR);
            Assert.AreEqual(result.StandardError, $"The RunCommand p cannot be found.\nPlease check the programming language configuration.");
        }
    }
}
