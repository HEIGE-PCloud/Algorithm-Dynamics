using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public ResultCode ResultCode { 
            get 
            {
                foreach (var result in Results)
                {
                    if (result.ResultCode != ResultCode.SUCCESS)
                        return result.ResultCode;
                }
                return ResultCode.SUCCESS;
            } 
        }

        public long CPUTime { get => _results.Max(r => r.CPUTime); }

        public long MemoryUsage { get => _results.Max(r => r.MemoryUsage); }

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
