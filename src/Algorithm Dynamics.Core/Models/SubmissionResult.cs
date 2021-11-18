using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class TestCaseResult
    {
        public int Id { get; set; }
        public TestCase TestCase { get; set; }
        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public int ExitCode { get; set; }
        public int CPUTime { get; set; }
        public int MemoryUsage { get; set; }
        public ResultCode resultCode { get; set; }
    }
    public class SubmissionResult
    {
        public int Id { get; set; }
        public Submission Submission { get; set; }
        public List<TestCaseResult> TestCaseResults { get; set; }
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
