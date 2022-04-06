using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        [TestMethod, TestCategory("TestRunCode")]
        public async Task TestRunCodeInputPy()
        {
            RunCodeResult result = await Judger.RunCode(APlusBPy, "3\n4\n", LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
            Assert.AreEqual("7\n", result.StandardOutput);
        }

        /// <summary>
        /// Test input processing
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestRunCode")]
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

        /// <summary>
        /// Test judge <see cref="TestCase"/>
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestTestCase")]
        public async Task TestTestCasePy()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");
            TestCase testCase = TestCase.Create("", "hello world\n", false);
            TestCaseResult result = await Judger.JudgeTestCase(helloWorldPy, testCase, LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual("hello world\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test judge <see cref="TestCase"/>
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestTestCase")]
        public async Task TestTestCaseCpp()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");
            TestCase testCase = TestCase.Create("", "hello world\n", false);
            TestCaseResult result = await Judger.JudgeTestCase(helloWorldCpp, testCase, LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual("hello world\n", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime < 2 * S && result.CPUTime > 0);
            Assert.IsTrue(result.MemoryUsage < 128 * MB && result.MemoryUsage > 0);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
        }

        /// <summary>
        /// Test Wrong Answer
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestWrongAnswer")]
        public async Task TestWrongAnswerPy()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");
            TestCase testCase = TestCase.Create("3\n4\n", "7", false);
            TestCaseResult result = await Judger.JudgeTestCase(WAPy, testCase, LanguageConfig.Python, 1 * S, 64 * MB);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime > 0 && result.CPUTime < 2 * S);
            Assert.IsTrue(result.MemoryUsage > 0 && result.MemoryUsage < 128 * MB);
            Assert.AreEqual(ResultCode.WRONG_ANSWER, result.ResultCode);
            Assert.AreNotEqual("7", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
        }

        /// <summary>
        /// Test Wrong Answer
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestWrongAnswer")]
        public async Task TestWrongAnswerCpp()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");
            TestCase testCase = TestCase.Create("3\n4\n", "7", false);
            TestCaseResult result = await Judger.JudgeTestCase(WACpp, testCase, LanguageConfig.Cpp, 1 * S, 64 * MB);
            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.CPUTime > 0 && result.CPUTime < 2 * S);
            Assert.IsTrue(result.MemoryUsage > 0 && result.MemoryUsage < 128 * MB);
            Assert.AreEqual(ResultCode.WRONG_ANSWER, result.ResultCode);
            Assert.AreNotEqual("7", result.StandardOutput);
            Assert.AreEqual("", result.StandardError);
        }

        /// <summary>
        /// Test Judge Problem
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestJudgeProblem")]
        public async Task TestJudgeProblemPy()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");

            // Generate 1000 A + B test cases
            Random rnd = new();
            List<TestCase> testCases = new();
            for (int i = 0; i < 100; i ++)
            {
                int a = rnd.Next(1000);
                int b = rnd.Next(1000);
                testCases.Add(TestCase.Create($"{a}\n{b}\n", $"{a + b}\n", false));
            }
            
            // Generate problem
            Problem problem = Problem.Create(Guid.NewGuid(), "", "", 1 * S, 64 * MB, Difficulty.Easy, testCases);

            // Generate submission
            User user = DatabaseHelper.CreateNewUser();
            Language language = DatabaseHelper.CreateLanguage("python");
            Submission submission = Submission.Create(APlusBPy, language, user, problem);
            SubmissionResult result = await Judger.JudgeProblem(submission, new Progress<int>());

            // Run tests
            Assert.AreEqual("Success", result.Result);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
            Assert.IsTrue(0 < result.CPUTime && result.CPUTime < 1 * S);
            Assert.IsTrue(0 < result.MemoryUsage && result.MemoryUsage < 128 * MB);
        }

        /// <summary>
        /// Test Judge Problem
        /// </summary>
        /// <returns></returns>
        [TestMethod, TestCategory("TestJudgeProblem")]
        public async Task TestJudgeProblemCpp()
        {
            DataAccess.InitializeDatabase($"{Guid.NewGuid()}.db");

            // Generate 1000 A + B test cases
            Random rnd = new();
            List<TestCase> testCases = new();
            for (int i = 0; i < 100; i++)
            {
                int a = rnd.Next(1000);
                int b = rnd.Next(1000);
                testCases.Add(TestCase.Create($"{a}\n{b}\n", $"{a + b}\n", false));
            }

            // Generate problem
            Problem problem = Problem.Create(Guid.NewGuid(), "", "", 1 * S, 64 * MB, Difficulty.Easy, testCases);

            // Generate submission
            User user = DatabaseHelper.CreateNewUser();
            Language language = DatabaseHelper.CreateLanguage("cpp");
            Submission submission = Submission.Create(APlusBCpp, language, user, problem);
            SubmissionResult result = await Judger.JudgeProblem(submission, new Progress<int>());

            // Run tests
            Assert.AreEqual("Success", result.Result);
            Assert.AreEqual(ResultCode.SUCCESS, result.ResultCode);
            Assert.IsTrue(0 < result.CPUTime && result.CPUTime < 1 * S);
            Assert.IsTrue(0 < result.MemoryUsage && result.MemoryUsage < 128 * MB);
        }
    }
}
