namespace Algorithm_Dynamics.Core.Models
{
    public class RunCodeResult
    {
        public RunCodeResult(string standardOutput = "", string standardError = "", int exitCode = 0, long cPUTime = 0, long memoryUsage = 0, ResultCode resultCode = ResultCode.SUCCESS)
        {
            StandardOutput = standardOutput;
            StandardError = standardError;
            ExitCode = exitCode;
            CPUTime = cPUTime;
            MemoryUsage = memoryUsage;
            ResultCode = resultCode;
        }

        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public int ExitCode { get; set; }
        public long CPUTime { get; set; }
        public long MemoryUsage { get; set; }
        public ResultCode ResultCode { get; set; }
    }
    public enum ResultCode
    {
        WRONG_ANSWER,
        SUCCESS,
        COMPILE_ERROR,
        TIME_LIMIT_EXCEEDED,
        MEMORY_LIMIT_EXCEEDED,
        RUNTIME_ERROR,
        SYSTEM_ERROR,
    }
}
