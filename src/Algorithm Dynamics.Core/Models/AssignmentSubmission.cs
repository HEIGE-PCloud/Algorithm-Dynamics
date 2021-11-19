using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class AssignmentSubmission
    {
        public Guid Uid { get; set; }
        public User Submitter { get; set; }
        public Assignment Assignment { get; set; }
        public AssignmentSubmissionStatus Status { get; set; }
        public List<Submission>  Submissions { get; set; }
    }
    public enum AssignmentSubmissionStatus
    {
        NotMarked,
        Marked,
        Returned
    }
}
