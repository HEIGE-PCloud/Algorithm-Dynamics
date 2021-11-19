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
        public string Name { get; set; }
        public ProblemStatus Status { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Description { get; set; }
        public int TimeLimit { get; set; }
        public int MemoryLimit { get; set; }
        public List<TestCase> TestCases { get; set; }
        public List<Tag> Tags { get; set; }
        public Problem(string name)
        {
            Name = name;
        }

        public Problem(int id, string name, ProblemStatus status, Difficulty difficulty, string description, int timeLimit, int memoryLimit, List<TestCase> testCases, List<Tag> tags)
        {
            Id = id;
            Name = name;
            Status = status;
            Difficulty = difficulty;
            Description = description;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            TestCases = testCases;
            Tags = tags;
        }
        public override string ToString()
        {
            return Name;
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
