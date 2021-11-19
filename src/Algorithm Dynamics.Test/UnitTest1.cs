using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithm_Dynamics.Core.Models;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestJudger
    {
        [TestMethod]
        [DataRow("print('hello world')", "",  1000, "hello world\n")]

        public async Task TestRunCode(string code, string input, int timeLimit, string expected)
        {
            Judger.SetSourceCodeFilePath("temp", "sol");
            RunCodeResult result = await Judger.RunCode(code, input, LanguageConfig.Python, timeLimit);
            Assert.AreEqual(result.StandardOutput, expected);
        }
    }
}
