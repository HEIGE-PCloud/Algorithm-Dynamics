using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Core.Models
{
    public class SubmissionResult
    {
        internal SubmissionResult(int id, Submission submission, List<TestCaseResult> results)
        {
            Id = id;
            Submission = submission;
            _results = results;
        }

        public int Id { get; set; }
        public Submission Submission { get; set; }

        private List<TestCaseResult> _results { get; set; }
        public ReadOnlyCollection<TestCaseResult> Results { get => _results.AsReadOnly(); }

        public override bool Equals(object obj)
        {
            if (obj is not SubmissionResult sub)
                return false;
            return Id == sub.Id && Equals(Submission, sub.Submission);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Submission, Results);
        }

        public static SubmissionResult Create(Submission submission, List<TestCaseResult> results)
        {
            var submissionResult = DataAccess.AddSubmissionResult(submission, results);
            if (results != null)
            {
                foreach (var testCaseResult in results)
                {
                    testCaseResult.SubmissionResultId = submissionResult.Id;
                }
            }
            else
            {
                submissionResult._results = new List<TestCaseResult>();
            }
            return submissionResult;
        }

        public void AddTestCaseResult(TestCaseResult testCaseResult)
        {
            testCaseResult.SubmissionResultId = Id;
            _results.Add(testCaseResult);
        }
    }
}
