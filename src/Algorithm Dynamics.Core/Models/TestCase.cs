using Algorithm_Dynamics.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static TestCase Create(string input, string output, bool isExample)
        {
            return DataAccess.AddTestCase(input, output, isExample);
        }
        public void Delete()
        {
            DataAccess.DeleteTestCase(this);
        }
        public override bool Equals(object obj)
        {
            var testCase = obj as TestCase;

            if (testCase == null)
                return false;
            return Id == testCase.Id && Input == testCase.Input && Output == testCase.Output && IsExample == testCase.IsExample;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Input, Output, IsExample);
        }
        private int _id;
        private string _input;
        private string _output;
        private bool _isExample;
        public int Id 
        {
            get => _id;
            private set => _id = value;
        }
        public string Input 
        { 
            get => _input;
            set
            {
                if (value == _input)
                {
                    _input = value;
                    DataAccess.EditTestCase(this, value, _output, _isExample);
                }
            }
        }
        public string Output 
        {
            get => _output;
            set
            {
                if (value != _output)
                {
                    _output = value;
                    DataAccess.EditTestCase(this, _input, value, _isExample);
                }
            }
        }
        public bool IsExample 
        { 
            get => _isExample;
            set
            {
                if (value != _isExample)
                {
                    _isExample = value;
                    DataAccess.EditTestCase(this, _input, _output, value);
                }
            }
        }
        internal int ProblemId
        {
            set
            {
                DataAccess.EditTestCase(this, _input, _output, _isExample, value);
            }
        }
    }
}
