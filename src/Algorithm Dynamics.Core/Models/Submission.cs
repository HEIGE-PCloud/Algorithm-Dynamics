using Algorithm_Dynamics.Core.Helpers;
using System;

namespace Algorithm_Dynamics.Core.Models
{
    public class Submission
    {
        internal Submission(int id, string code, DateTime submittedTime, Language language, User submitter, Problem problem)
        {
            Id = id;
            Code = code;
            SubmittedTime = submittedTime;
            Language = language;
            Submitter = submitter;
            Problem = problem;
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime SubmittedTime { get; set; }
        public Language Language { get; set; }
        public User Submitter { get; set; }
        public Problem Problem { get; set; }

        public static Submission Create(string code, Language language, User user, Problem problem)
        {
            return DataAccess.AddSubmission(code, DateTime.Now, language, user, problem);
        }
        public override bool Equals(object obj)
        {
            if (obj is not Submission sub)
                return false;
            //return Id == sub.Id && Code == sub.Code && SubmittedTime == sub.SubmittedTime && Language == sub.Language && Submitter == sub.Submitter && Problem == sub.Problem;
            return Id == sub.Id && Code == sub.Code && SubmittedTime == sub.SubmittedTime && Equals(Language, sub.Language) && Equals(Problem, sub.Problem) && Equals(Submitter, sub.Submitter);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Code, SubmittedTime, Language, Submitter, Problem);
        }
    }
}
