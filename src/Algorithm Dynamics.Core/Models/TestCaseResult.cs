namespace Algorithm_Dynamics.Core.Models
{
    public class TestCaseResult : RunCodeResult
    {
        public int Id { get; set; }
        public TestCase TestCase { get; set; }
        public TestCaseResult()
        {
        }
        public TestCaseResult(TestCase testCase)
        {
            TestCase = testCase;
        }
        public TestCaseResult(TestCase testCase, RunCodeResult result)
        {
            StandardOutput = result.StandardOutput;
            StandardError = result.StandardError;
            ExitCode = result.ExitCode;
            CPUTime = result.CPUTime;
            MemoryUsage = result.MemoryUsage;
            ResultCode = result.ResultCode;
            TestCase = testCase;
        }
    }
}
