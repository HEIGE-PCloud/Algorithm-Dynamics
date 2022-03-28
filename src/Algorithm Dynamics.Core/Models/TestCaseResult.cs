using Algorithm_Dynamics.Core.Helpers;

namespace Algorithm_Dynamics.Core.Models
{
    public class TestCaseResult : RunCodeResult
    {
        internal TestCaseResult(
            int id,
            string standardOutput,
            string standardError,
            int exitCode,
            long cPUTime,
            long memoryUsage,
            ResultCode resultCode
        )
        {
            Id = id;
            StandardOutput = standardOutput;
            StandardError = standardError;
            ExitCode = exitCode;
            CPUTime = cPUTime;
            MemoryUsage = memoryUsage;
            ResultCode = resultCode;
        }
        public int Id { get; private set; }
        internal int SubmissionResultId { set => DataAccess.EditTestCaseResult(Id, value); }
        
        /// <summary>
        /// Create a new test case result
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        internal static TestCaseResult Create(RunCodeResult r)
        {
            return DataAccess.AddTestCaseResult(
                r.StandardOutput,
                r.StandardError,
                r.ExitCode,
                r.CPUTime,
                r.MemoryUsage,
                r.ResultCode,
                null);
        }
        
        /// <summary>
        /// Delete the current test case result from the database
        /// </summary>
        internal void Delete()
        {
            DataAccess.DeleteTestCaseResult(Id);
        }

        public override bool Equals(object obj)
        {
            if (obj is not TestCaseResult result)
                return false;
            return
                Id == result.Id &&
                StandardOutput == result.StandardOutput &&
                StandardError == result.StandardError &&
                ExitCode == result.ExitCode &&
                CPUTime == result.CPUTime &&
                MemoryUsage == result.MemoryUsage &&
                ResultCode == result.ResultCode;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
