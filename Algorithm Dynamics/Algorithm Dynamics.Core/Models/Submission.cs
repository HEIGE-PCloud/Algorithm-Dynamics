using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Submission
    {
        public Problem SubmittedProblem { get; set; }
        public string UserCode { get; set; }
        public DateTime SubmittedTime { get; set; }
    }
}
