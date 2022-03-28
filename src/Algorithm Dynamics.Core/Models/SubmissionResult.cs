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

        public int Id { get; private set; }
        public Submission Submission { get; private set; }

        private List<TestCaseResult> _results;
        public ReadOnlyCollection<TestCaseResult> Results { get => _results.AsReadOnly(); }
        
        /// <summary>
        /// The result code of a submission result 
        /// is the first non-success result code
        /// of a test case result
        /// </summary>
        public ResultCode ResultCode
        {
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

        /// <summary>
        /// Return a string representation of the result code
        /// </summary>
        public string Result
        {
            get => ResultCode switch
                {
                    ResultCode.WRONG_ANSWER => "Wrong Answer",
                    ResultCode.SUCCESS => "Success",
                    ResultCode.COMPILE_ERROR => "Compile Error",
                    ResultCode.TIME_LIMIT_EXCEEDED => "Time Limit Exceeded",
                    ResultCode.MEMORY_LIMIT_EXCEEDED => "Memory Limit Exceeded",
                    ResultCode.RUNTIME_ERROR => "Runtime Error",
                    _ => "System Error",
                };
        }

        /// <summary>
        /// The CPU time of a submission result 
        /// is the max CPU time of a test case result
        /// </summary>
        public long CPUTime { get => _results.Max(r => r.CPUTime); }
        public string CPUTimeAsString { get => $"{CPUTime} ms"; }

        /// <summary>
        /// The memory usage of a submission result 
        /// is the max memory usage of a test case result
        /// </summary>
        public long MemoryUsage { get => _results.Max(r => r.MemoryUsage); }
        public string MemoryUsageAsString { get => $"{MemoryUsage / 1024 / 1024} MB"; }

        /// <summary>
        /// The standard error of a submission result 
        /// is the first stderr from a test case result
        /// </summary>
        public string StandardError
        {
            get
            {
                foreach (var result in Results)
                {
                    if (!string.IsNullOrWhiteSpace(result.StandardError))
                        return result.StandardError;
                }
                return "";
            }
        }

        /// <summary>
        /// Get all SubmissionResults from the database
        /// </summary>
        public static List<SubmissionResult> All { get => DataAccess.GetAllSubmissionResults(); }

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
        
        /// <summary>
        /// Create a new submission result in the database
        /// </summary>
        /// <param name="submission"></param>
        /// <param name="results"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete all TestCaseResult associates
        /// and self destory
        /// </summary>
        public void Delete()
        {
            while (Results.Count > 0)
            {
                RemoveTestCaseResult(Results.First());
            }
            DataAccess.DeleteSubmissionResult(Id);
        }

        /// <summary>
        /// Add a new TestCaseResult to the SubmissionResult
        /// and save to the database.
        /// </summary>
        /// <param name="testCaseResult"></param>
        public void AddTestCaseResult(TestCaseResult testCaseResult)
        {
            testCaseResult.SubmissionResultId = Id;
            _results.Add(testCaseResult);
        }

        /// <summary>
        /// Remvoe a TestCaseResult from the database
        /// </summary>
        /// <param name="testCaseResult"></param>
        public void RemoveTestCaseResult(TestCaseResult testCaseResult)
        {
            _results.Remove(testCaseResult);
            testCaseResult.Delete();
        }
    }
}
