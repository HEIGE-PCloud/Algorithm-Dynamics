using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace Algorithm_Dynamics.Core.Models
{
    public class TestCase
    {
        internal TestCase(int id, string input, string output, bool isExample)
        {
            _id = id;
            _input = input;
            _output = output;
            _isExample = isExample;
        }

        private int _id;
        [JsonIgnore]
        public int Id { get => _id; private set => _id = value; }

        private string _input;
        public string Input
        {
            get => _input;
            set
            {
                if (value == _input)
                {
                    _input = value;
                    UpdateDatabase();
                }
            }
        }

        private string _output;
        public string Output
        {
            get => _output;
            set
            {
                if (value != _output)
                {
                    _output = value;
                    UpdateDatabase();
                }
            }
        }

        private bool _isExample;
        public bool IsExample
        {
            get => _isExample;
            set
            {
                if (value != _isExample)
                {
                    _isExample = value;
                    UpdateDatabase();
                }
            }
        }

        /// <summary>
        /// Set the problem id of the testcase
        /// </summary>
        internal int ProblemId 
        { 
            set => DataAccess.EditTestCase(_id, _input, _output, _isExample, value); 
        }
        
        /// <summary>
        /// Create a new <see cref="TestCase"/> and save it into the databse.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="isExample"></param>
        /// <returns></returns>
        public static TestCase Create(string input, string output, bool isExample)
        {
            return DataAccess.AddTestCase(input, output, isExample);
        }

        /// <summary>
        /// Delete the <see cref="TestCase"/> from Database
        /// </summary>
        internal void Delete()
        {
            DataAccess.DeleteTestCase(_id);
        }

        private void UpdateDatabase()
        {
            DataAccess.EditTestCase(_id, _input, _output, _isExample);
        }

        /// <summary>
        /// Determine whether two testcases are the same
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is not TestCase testCase)
                return false;
            return Id == testCase.Id && Input == testCase.Input && Output == testCase.Output && IsExample == testCase.IsExample;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Input, Output, IsExample);
        }
    }
}
