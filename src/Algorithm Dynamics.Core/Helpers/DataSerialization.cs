using Algorithm_Dynamics.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Algorithm_Dynamics.Core.Helpers
{
    public static class DataSerialization
    {
        /// <summary>
        /// The base Object container that holds all the other data objects
        /// It adds <see cref="FileType"/> and <see cref="DataType"/> info to the data.
        /// </summary>
        private class ExportObject
        {
            public ExportObject(string dataType, object data)
            {
                DataType = dataType;
                Data = data;
            }

            public string FileType { get; set; } = "Algorithm Dynamics Exported Data";
            public string DataType { get; set; }
            public object Data { get; set; }
        }

        /// <summary>
        /// The base test case model to hold the import data.
        /// </summary>
        private class BaseTestCase
        {
            public string Input { get; set; }
            public string Output { get; set; }
            public bool IsExample { get; set; }
        }

        /// <summary>
        /// The base tag model to hold the import data.
        /// </summary>
        private class BaseTag
        {
            public string Name { get; set; }
        }

        /// <summary>
        /// The base problem model to hold the import data
        /// </summary>
        private class BaseProblem
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

        /// <summary>
        /// The base problem list 
        /// </summary>
        private class BaseProblemList
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<BaseProblem> Problems { get; set; }
        }

        /// <summary>
        /// Convert a problem instance into a JSON string ready to be exported.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        public static string SerializeProblem(Problem problem)
        {
            return JsonSerializer.Serialize(new ExportObject("Problem", problem));
        }

        /// <summary>
        /// Convert a problem list instance into a JSON string ready to be exported.
        /// </summary>
        /// <param name="problemList"></param>
        /// <returns></returns>
        public static string SerializeProblemList(ProblemList problemList)
        {
            return JsonSerializer.Serialize(new ExportObject("ProblemList", problemList));
        }

        /// <summary>
        /// Get data type (problem/problem list) of an import data file.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <exception cref="FormatException">The data format is invalid</exception>
        public static string GetDataType(string str)
        {
            var @base = JsonSerializer.Deserialize<ExportObject>(str);
            if (@base.FileType != "Algorithm Dynamics Exported Data")
                throw new FormatException($"The FileType {@base.FileType} is invalid.");

            if (@base.DataType != "Problem" && @base.DataType != "ProblemList")
                throw new FormatException($"The DataType {@base.DataType} is invalid.");
            return @base.DataType;
        }

        /// <summary>
        /// Convert a valid JSON string into a problem and save it to the database
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Problem DeserializeProblem(string str)
        {
            // Convert the string into the base model
            var @base = JsonSerializer.Deserialize<ExportObject>(str);

            // Read the problem data from the base data
            var baseProblem = JsonSerializer.Deserialize<BaseProblem>(@base.Data.ToString());
            
            // Create test cases and tags first
            List<TestCase> testCases = new();
            List<Tag> tags = new();
            foreach (BaseTestCase t in baseProblem.TestCases)
            {
                testCases.Add(TestCase.Create(t.Input, t.Output, t.IsExample));
            }
            foreach (BaseTag t in baseProblem.Tags)
            {
                tags.Add(Tag.Create(t.Name));
            }

            // Create the problem next and return it
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

        /// <summary>
        /// Convert a valid JSON string into a problem list and save it to the database
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ProblemList DeserializeProblemList(string str)
        {
            // Convert the string into the base model
            var @base = JsonSerializer.Deserialize<ExportObject>(str);

            // Read the problem list data from the base data
            var baseProblemList = JsonSerializer.Deserialize<BaseProblemList>(@base.Data.ToString());
            List<Problem> problems = new();
            foreach (BaseProblem p in baseProblemList.Problems)
            {
                // Create problem from the problem list data
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

                // Add to the problem list
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
