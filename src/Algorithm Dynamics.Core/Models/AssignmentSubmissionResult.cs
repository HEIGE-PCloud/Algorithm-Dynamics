using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Models
{
    public class AssignmentSubmissionResult
    {
        public Guid Uid { get; set; }
        public AssignmentSubmission AssignmentSubmission { get; set; }
        public List<SubmissionResult> Results { get; set; }
        public void Add(SubmissionResult submissionResult)
        {
            Results.Add(submissionResult);
        }
    }
}
