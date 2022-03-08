using Algorithm_Dynamics.Core.Helpers;

namespace Algorithm_Dynamics.Core.Models
{
    public class TestCaseResult : RunCodeResult
    {
        public int Id { get; set; }
        internal TestCaseResult(int id, string standardOutput, string standardError, int exitCode, long cPUTime, long memoryUsage, ResultCode resultCode)
        {
            Id = id;
            StandardOutput = standardOutput;
            StandardError = standardError;
            ExitCode = exitCode;
            CPUTime = cPUTime;
            MemoryUsage = memoryUsage;
            ResultCode = resultCode;
        }
        internal int SubmissionResultId
        {
            set
            {
                DataAccess.EditTestCaseResult(Id, value);
            }
        }
        public static TestCaseResult Create(RunCodeResult r, int? submissionResultId)
        {
            return DataAccess.AddTestCaseResult(r.StandardOutput, r.StandardError, r.ExitCode, r.CPUTime, r.MemoryUsage, r.ResultCode, submissionResultId);
        }
    }
}
