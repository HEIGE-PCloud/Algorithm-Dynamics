using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class AssignmentSubmissionResult
    {
        public Guid Uid { get; set; }
        public AssignmentSubmission AssignmentSubmission { get; set; }
        public List<SubmissionResult> SubmissionResults { get; set; }
    }
}
