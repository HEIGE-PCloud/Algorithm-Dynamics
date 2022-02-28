using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }
        public long MemoryLimit { get; set; }
        public ProblemStatus Status { get; set; }
        public Difficulty Difficulty { get; set; }
        public List<TestCase> TestCases { get; set; }
        public List<Tag> Tags { get; set; }

        public Problem(int id, Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases, List<Tag> tags)
        {
            Id = id;
            Uid = uid;
            Name = name;
            Description = description;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            Status = status;
            Difficulty = difficulty;
            TestCases = testCases;
            Tags = tags;
        }

        public override bool Equals(object obj)
        {
            var problem = obj as Problem;

            if (problem == null)
                return false;
            return problem.Id == Id && problem.Uid == Uid && problem.Name == Name && problem.Description == Description;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Uid, Name, Description);
        }
    }
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
    }
    public enum ProblemStatus
    {
        Todo,
        Solved,
        Attempted,
    }
}
