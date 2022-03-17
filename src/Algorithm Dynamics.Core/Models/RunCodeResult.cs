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
        public string Result
        {
            get
            {
                return ResultCode switch
                {
                    ResultCode.WRONG_ANSWER => "Wrong Answer",
                    ResultCode.SUCCESS => "Success",
                    ResultCode.COMPILE_ERROR => "Compile Error",
                    ResultCode.TIME_LIMIT_EXCEEDED => "Time Limit Exceed",
                    ResultCode.MEMORY_LIMIT_EXCEEDED => "Memory Limit Exceed",
                    ResultCode.RUNTIME_ERROR => "Runtime Error",
                    _ => "System Error",
                };
            }
        }
    }
}
