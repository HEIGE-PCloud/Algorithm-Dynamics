using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithm_Dynamics.Core.Models;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestCaseResult result = new();
            result.StandardOutput = "Hello World";
            Assert.AreEqual("Hello World", result.StandardOutput);
        }
        [TestMethod]
        public async Task TestMethod2()
        {
            Judger.SetSourceCodeFilePath("temp", "sol");
            TestCaseResult s = await Judger.RunCode("print('hello world')", "", LanguageConfig.Python, 1000);
            Assert.AreEqual(s.StandardOutput, "hello world\n");
        }
        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
