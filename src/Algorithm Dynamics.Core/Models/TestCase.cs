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
        public TestCase(int id, string input, string output, bool isExample)
        {
            Id = id;
            Input = input;
            Output = output;
            IsExample = isExample;
        }
        private TestCase(string input, string output, bool isExample)
        {
            Input = input;
            Output = output;
            IsExample = isExample;
        }
        public static TestCase Create(string input, string output, bool isExample)
        {
            return DataAccess.AddTestCase(new(input, output, isExample));
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
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
    }
}
