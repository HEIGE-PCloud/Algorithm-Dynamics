using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Algorithm_Dynamics.Core.Models
{
    public class Problem
    {
        internal Problem(
            int id,
            Guid uid,
            string name,
            string description,
            int timeLimit,
            long memoryLimit,
            ProblemStatus status,
            Difficulty difficulty,
            List<TestCase> testCases,
            List<Tag> tags)
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

        private int _id;
        [JsonIgnore]
        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        private Guid _uid;
        public Guid Uid
        {
            get => _uid;
            private set => _uid = value;
        }

        private string _name;
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

        private string _description;
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

        private int _timeLimit;
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

        private long _memoryLimit;
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

        private ProblemStatus _status;
        [JsonIgnore]
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
        [JsonIgnore]
        public string StatusAsString
        {
            get
            {
                return Status switch
                {
                    ProblemStatus.Todo => "Todo",
                    ProblemStatus.Solved => "Todo",
                    _ => "Todo",
                };
            }
        }

        private Difficulty _difficulty;
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
        [JsonIgnore]
        public string DifficultyAsString
        {
            get
            {
                return Difficulty switch
                {
                    Difficulty.Easy => "Easy",
                    Difficulty.Medium => "Medium",
                    _ => "Hard",
                };
            }
        }
        
        /// <summary>
        /// A list of <see cref="TestCase"/> belongs to this <see cref="Problem"/>
        /// </summary>
        private List<TestCase> _testCases;
        public ReadOnlyCollection<TestCase> TestCases { get => _testCases.AsReadOnly(); }

        /// <summary>
        /// A list of <see cref="Tag"/> belongs to this <see cref="Problem"/>
        /// </summary>
        private List<Tag> _tags;
        public ReadOnlyCollection<Tag> Tags { get => _tags.AsReadOnly(); }

        /// <summary>
        /// Return all tags as a string in the format "Tag1, Tag2, Tag3"
        /// </summary>
        [JsonIgnore]
        public string TagsAsString
        {
            get
            {
                string str = "";
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

        /// <summary>
        /// Return the first tag as string if there exists any
        /// Or an empty string if there is no tag
        /// </summary>
        [JsonIgnore]
        public string TagAsString
        {
            get
            {
                string str = "";
                if (_tags.Count > 0)
                {
                    str = _tags[0].Name;
                }
                return str;
            }
        }

        /// <summary>
        /// Return a list of all <see cref="Problem"/> in the database.
        /// </summary>
        public static List<Problem> All { get => DataAccess.GetAllProblems(); }


        /// <summary>
        /// Create a new <see cref="Problem"/> and save it to the database.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="timeLimit"></param>
        /// <param name="memoryLimit"></param>
        /// <param name="difficulty"></param>
        /// <param name="testCases"></param>
        /// <param name="tags"></param>
        /// <returns>An instance of the Problem you just created</returns>
        public static Problem Create(
            Guid uid,
            string name,
            string description,
            int timeLimit,
            long memoryLimit,
            Difficulty difficulty,
            List<TestCase> testCases = null,
            List<Tag> tags = null)
        {
            // Create record for Problem
            var problem = DataAccess.AddProblem(uid,
                name,
                description,
                timeLimit,
                memoryLimit,
                ProblemStatus.Todo,
                difficulty,
                testCases,
                tags);

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

        /// <summary>
        /// Update database, save all the data in attributes into the database.
        /// </summary>
        private void UpdateDatabase() => DataAccess.EditProblem(
            _id,
            _name,
            _description,
            _timeLimit,
            _memoryLimit,
            _status,
            _difficulty);

        /// <summary>
        /// Add an existing <see cref="TestCase"/> to the problem
        /// </summary>
        /// <param name="testCase"></param>
        public void AddTestCase(TestCase testCase)
        {
            testCase.ProblemId = Id;
            _testCases.Add(testCase);
        }
        
        /// <summary>
        /// Remove an existing <see cref="TestCase"/> from the <see cref="Problem"/>
        /// and delete it from the database.
        /// </summary>
        /// <param name="testCase"></param>
        public void RemoveTestCase(TestCase testCase)
        {
            testCase.Delete();
            _testCases.Remove(testCase);
        }

        /// <summary>
        /// Add an existing <see cref="Tag"/> to the <see cref="Problem"/>
        /// </summary>
        /// <param name="tag"></param>
        public void AddTag(Tag tag)
        {
            DataAccess.AddTagRecord(Id, tag.Id);
            _tags.Add(tag);
        }

        /// <summary>
        /// Delete an existing <see cref="Tag"/> from the <see cref="Problem.Tags"/>.
        /// Delete the <see cref="Tag"/> from the database if it does not attach to any
        /// other problems.
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(Tag tag)
        {
            // Delete the current record
            tag.DeleteRecord(Id);
            // If not belong to other problem
            // delete the tag completely
 
            if (DataAccess.TagRecordExists(tag.Id) == false)
                tag.Delete();
            
            // Delete from _tags
            _tags.Remove(tag);
        }

        /// <summary>
        /// Delete the current problem safely
        /// </summary>
        public void Delete()
        {
            // Delete all testcases
            while (_testCases.Count != 0) RemoveTestCase(_testCases[0]);
            // Delete all tags
            while (_tags.Count != 0) RemoveTag(_tags[0]);
            // Delete all existence in problem lists
            ProblemList.All.FindAll(list => list.Problems.Contains(this))
                .ForEach(list => list.RemoveProblem(this));
            // Delete all related submissions
            Submission.All.FindAll(submission => submission.Problem.Id == Id)
                .ForEach(submission => submission.Delete());
            // Self destory at the end
            DataAccess.DeleteProblem(_id);
        }

        /// <summary>
        /// Attach the problem to a problem list
        /// </summary>
        /// <param name="problemListId"></param>
        public void AttachTo(int problemListId)
        {
            DataAccess.AddProblemListRecord(problemListId, Id);
        }

        public override bool Equals(object obj)
        {
            // If not problem, not equal
            if (obj is not Problem problem)
            {
                return false;
            }

            // If Id/Uid/Name/Description not equal, not equal
            if ((problem.Id == Id
                && problem.Uid == Uid
                && problem.Name == Name
                && problem.Description == Description) == false)
            {
                return false;
            }

            // If test case count not equal, not equal
            if (problem.TestCases.Count != TestCases.Count)
            {
                return false;
            }

            // If tag count not equal, not equal
            if (problem.Tags.Count != Tags.Count)
            {
                return false;
            }

            // Compare test cases one by one
            for (int i = 0; i < TestCases.Count; i++)
            {
                if (Equals(TestCases[i], problem.TestCases[i]) == false)
                {
                    return false;
                }
            }

            // Compare tags one by one
            for (int i = 0; i < Tags.Count; i++)
            {
                if (Equals(Tags[i], problem.Tags[i]) == false)
                {
                    return false;
                }
            }

            // Otherwise, equal
            return true;
        }
 
        public override int GetHashCode()
            => HashCode.Combine(Id, Uid, Name, Description);

        public override string ToString()
            => Name;
    }
}
