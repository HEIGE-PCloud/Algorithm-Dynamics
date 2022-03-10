using Algorithm_Dynamics.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static class DataSerialization
    {
        public class BaseTestCase
        {
            public string Input { get; set; }
            public string Output { get; set; }
            public bool IsExample { get; set; }
        }

        public class BaseTag
        {
            public string Name { get; set; }
        }

        public class BaseProblem
        {
            public string Uid { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int TimeLimit { get; set; }
            public int MemoryLimit { get; set; }
            public int Difficulty { get; set; }
            public List<BaseTestCase> TestCases { get; set; }
            public List<BaseTag> Tags { get; set; }
        }

        public static string SerializeProblem(Problem problem)
        {
            return JsonSerializer.Serialize(problem);
        }

        public static Problem DeserializeProblem(string str)
        {
            var baseProblem = JsonSerializer.Deserialize<BaseProblem>(str);
            List<TestCase> testCases = new();
            List<Tag> tags = new();
            foreach(BaseTestCase t in baseProblem.TestCases)
            {
                testCases.Add(TestCase.Create(t.Input, t.Output, t.IsExample));
            }
            foreach(BaseTag t in baseProblem.Tags)
            {
                tags.Add(Tag.Create(t.Name));
            }
            return Problem.Create(
                Guid.Parse(baseProblem.Uid), 
                baseProblem.Name, 
                baseProblem.Description, 
                baseProblem.TimeLimit, 
                baseProblem.MemoryLimit, 
                (Difficulty)baseProblem.Difficulty, 
                testCases, 
                tags);
        }
    }
}
