using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime SubmittedTime { get; set; }
        public Language Language { get; set; }
        public User Submitter { get; set; }
        public Problem Problem { get; set; }
    }
}
