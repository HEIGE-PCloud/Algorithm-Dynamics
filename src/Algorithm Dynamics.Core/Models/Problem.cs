﻿using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Core.Models
{
    public class Problem
    {
        private int _id;
        private Guid _uid;
        private string _name;
        private string _description;
        private int _timeLimit;
        private long _memoryLimit;
        private ProblemStatus _status;
        private Difficulty _difficulty;
        private void UpdateDatabase()
        {
            DataAccess.EditProblem(_id, _name, _description, _timeLimit, _memoryLimit, _status, _difficulty);
        }
        public int Id 
        { 
            get => _id; 
            private set => _id = value;
        }
        public Guid Uid 
        { 
            get => _uid;
            private set => _uid = value;
        }
        public string Name 
        { 
            get => _name; 
            set
            {
                if (_name != value)
                {
                    _name = value;
                    UpdateDatabase();
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    UpdateDatabase();
                }
            }
        }
        public int TimeLimit 
        { 
            get => _timeLimit;
            set 
            {
                if (_timeLimit != value)
                {
                    _timeLimit = value;
                    UpdateDatabase();
                }
            }
        }
        public long MemoryLimit 
        { 
            get => _memoryLimit;
            set
            {
                if (_memoryLimit != value)
                {
                    _memoryLimit = value;
                    UpdateDatabase();
                }
            }
        }
        public ProblemStatus Status 
        { 
            get => _status;
            set
            { 
                if (_status != value)
                {
                    _status = value;
                    UpdateDatabase();
                }
            }
        }
        public Difficulty Difficulty 
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    UpdateDatabase();
                }
            }
        }
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
                string str = " ";
                if (_tags.Count > 0)
                {
                    for (int i = 0; i < _tags.Count - 1; i++)
                    {
                        str += _tags[i].Name + ", ";
                    }
                    str += _tags[^1].Name;
                }
                return str;
            }
        }
        private List<TestCase> _testCases;
        public ReadOnlyCollection<TestCase> TestCases
        {
            get => _testCases.AsReadOnly();
        }
        private List<Tag> _tags;
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
            tag.DeleteRecord(Id);
            if (DataAccess.TagRecordExists(tag.Id) == false)
                tag.Delete();
            _tags.Remove(tag);
        }
        internal Problem(int id, Guid uid, string name, string description, int timeLimit, long memoryLimit, ProblemStatus status, Difficulty difficulty, List<TestCase> testCases, List<Tag> tags)
        {
            _id = id;
            _uid = uid;
            _name = name;
            _description = description;
            _timeLimit = timeLimit;
            _memoryLimit = memoryLimit;
            _status = status;
            _difficulty = difficulty;
            _testCases = testCases;
            _tags = tags;
        }

        public static Problem Create(string name, string description, int timeLimit, long memoryLimit, Difficulty difficulty, List<TestCase> testCases = null, List<Tag> tags = null)
        {
            // Create record for Problem
            var problem = DataAccess.AddProblem(Guid.NewGuid(), name, description, timeLimit, memoryLimit, ProblemStatus.Todo, difficulty, testCases, tags);
            
            // Add testcases to problem
            if (testCases != null)
            {
                foreach (var testCase in testCases)
                {
                    testCase.ProblemId = problem.Id;
                }
            }
            else
            {
                problem._testCases = new() { };
            }

            // Add tags to problem
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    tag.AttachTo(problem.Id);
                }
            }
            else
            {
                problem._tags = new() { };
            }

            return problem;
        }

        public override bool Equals(object obj)
        {
            Problem problem = obj as Problem;
            if (problem == null)
                return false;
            if ((problem.Id == Id && problem.Uid == Uid && problem.Name == Name && problem.Description == Description) == false)
                return false;
            if (problem.TestCases.Count != TestCases.Count)
                return false;
            if (problem.Tags.Count != Tags.Count)
                return false;
            for (int i = 0; i < TestCases.Count; i++)
            {
                if (Equals(TestCases[i], problem.TestCases[i]) == false)
                {
                    return false;
                }
            }
            for (int i = 0; i < Tags.Count; i++)
            {
                if (Equals(Tags[i], problem.Tags[i]) == false)
                {
                    return false;
                }
            }
            return true;
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
