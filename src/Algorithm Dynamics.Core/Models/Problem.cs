﻿using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
        public string str_Difficulty
        {
            get
            {
                if (Difficulty == Difficulty.Easy)
                    return "Easy";
                else if (Difficulty == Difficulty.Medium)
                    return "Medium";
                else
                    return "Hard";
            }
        }
        public string str_Status
        {
            get
            {
                if (Status == ProblemStatus.Todo)
                    return "Todo";
                else if (Status == ProblemStatus.Attempted)
                    return "Attempted";
                else
                    return "Done";
            }
        }
        public string str_Tags
        {
            get
            {
                string str = "";
                foreach (var tag in _tags)
                {
                    str += tag.Name + ", ";
                }
                return str;
            }
        }
        private List<TestCase> _testCases;
        public ReadOnlyCollection<TestCase> TestCases
        {
            get => _testCases.AsReadOnly();
        }
        public List<Tag> _tags;
        public ReadOnlyCollection<Tag> Tags
        {
            get => _tags.AsReadOnly();
        }
        public void AddTestCase(TestCase testCase)
        {
            testCase.ProblemId = Id;
            _testCases.Add(testCase);
        }
        public void AddTag(Tag tag)
        {
            DataAccess.AddTagRecord(Id, tag.Id);
            _tags.Add(tag);
        }
        public void RemoveTestCase(TestCase testCase)
        {
            testCase.Delete();
            _testCases.Remove(testCase);
        }
        public void RemoveTag(Tag tag)
        {
            DataAccess.DeleteTagRecord(Id, tag.Id);
            _tags.Remove(tag);
        }
        internal Problem(int id, Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases, List<Tag> tags)
        {
            Id = id;
            Uid = uid;
            Name = name;
            Description = description;
            TimeLimit = timeLimit;
            MemoryLimit = memoryLimit;
            Status = status;
            Difficulty = difficulty;
            _testCases = testCases;
            _tags = tags;
        }

        public static Problem Create(Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases = null, List<Tag> tags = null)
        {
            // Create record for Problem
            var problem = DataAccess.AddProblem(uid, name, description, timeLimit, memoryLimit, status, difficulty, testCases, tags);
            
            // Add testcases to problem
            if (testCases != null)
            {
                foreach (var testCase in testCases)
                {
                    testCase.ProblemId = problem.Id;
                }
            }

            // Add tags to problem
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    tag.AttachTo(problem.Id);
                }
            }

            return problem;
        }

        public override bool Equals(object obj)
        {
            Problem problem = obj as Problem;

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
