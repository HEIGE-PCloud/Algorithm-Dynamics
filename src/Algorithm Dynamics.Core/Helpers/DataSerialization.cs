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
        public class ExportObject
        {
            public ExportObject(string dataType, object data)
            {
                DataType = dataType;
                Data = data;
            }

            public string FileType { get; } = "Algorithm Dynamics Exported Data";
            public string DataType { get; set; }
            public object Data { get; set; }
        }
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

        public class BaseProblemList
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<BaseProblem> Problems { get; set; }
        }

        public static string SerializeProblem(Problem problem)
        {
            return JsonSerializer.Serialize(new ExportObject("Problem", problem));
        }

        public static string SerializeProblemList(ProblemList problemList)
        {
            return JsonSerializer.Serialize(new ExportObject("ProblemList", problemList));
        }
        public static string GetDataType(string str)
        {
            var @base = JsonSerializer.Deserialize<ExportObject>(str);
            if (@base.FileType != "Algorithm Dynamics Exported Data")
                throw new FormatException($"The FileType {@base.FileType} is invalid.");

            if (@base.DataType != "Problem" || @base.DataType != "ProblemList")
                throw new FormatException($"The DataType {@base.DataType} is invalid.");
            return @base.DataType;
        }
        public static Problem DeserializeProblem(string str)
        {
            var @base = JsonSerializer.Deserialize<ExportObject>(str);
            var baseProblem = JsonSerializer.Deserialize<BaseProblem>(@base.Data.ToString());
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

        public static ProblemList DeserializeProblemList(string str)
        {
            var @base = JsonSerializer.Deserialize<ExportObject>(str);
            var baseProblemList = JsonSerializer.Deserialize<BaseProblemList>(@base.Data.ToString());
            List<Problem> problems = new();
            foreach(BaseProblem p in baseProblemList.Problems)
            {
                List<TestCase> testCases = new();
                List<Tag> tags = new();
                foreach (BaseTestCase t in p.TestCases)
                {
                    testCases.Add(TestCase.Create(t.Input, t.Output, t.IsExample));
                }
                foreach (BaseTag t in p.Tags)
                {
                    tags.Add(Tag.Create(t.Name));
                }
                problems.Add(
                    Problem.Create(
                        Guid.Parse(p.Uid),
                        p.Name,
                        p.Description,
                        p.TimeLimit,
                        p.MemoryLimit,
                        (Difficulty)p.Difficulty,
                        testCases,
                        tags
                    )
                );
            }
            return ProblemList.Create(baseProblemList.Name, baseProblemList.Description, problems);
        }
    }
}
