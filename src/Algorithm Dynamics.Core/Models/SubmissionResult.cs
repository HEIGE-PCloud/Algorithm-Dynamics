using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class SubmissionResult
    {
        public Submission UserSubmission { get; set; }
        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public string ExitCode { get; set; }
        public int CPUTime { get; set; }
        public long Memory { get; set; }
        public ResultCode UserResultCode { get; set; }
        public enum ResultCode
        {
            WRONG_ANSWER,
            SUCCESS,
            COMPILE_ERROR,
            TIME_LIMIT_EXCEEDED,
            MEMORY_LIMIT_EXCEEDED,
            RUNTIME_ERROR,
            SYSTEM_ERROR
        }
        
    }
}
