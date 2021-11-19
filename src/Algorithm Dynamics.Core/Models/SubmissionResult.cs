using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class SubmissionResult
    {
        public int Id { get; set; }
        public Submission Submission { get; set; }
        public List<TestCaseResult> Results { get; set; }
        public void Add(TestCaseResult testCaseResult)
        {
            Results.Add(testCaseResult);
        }
    }
}
