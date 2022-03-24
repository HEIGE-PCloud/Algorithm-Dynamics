using Algorithm_Dynamics.Core.Models;
using System;

namespace Algorithm_Dynamics.Test
{
    internal static class DatabaseHelper
    {
        static int counter = 1;
        const int MB = 1024 * 1024;

        /// <summary>
        /// Create a random new <see cref="Problem"/> without any testcase or tag and save it into the database.
        /// </summary>
        /// <returns></returns>
        internal static Problem CreateNewProblem()
        {
            Problem problem = Problem.Create(Guid.NewGuid(), $"Test Problem {counter}", $"Description {counter}", 1000 * counter, 16 * MB * counter, Difficulty.Easy);
            counter++;
            return problem;
        }

        internal static Tag CreateNewTag()
        {
            Tag tag = Tag.Create($"Tag {counter++}");
            return tag;
        }

        internal static TestCase CreateNewTestCase()
        {
            TestCase testCase = TestCase.Create($"Input {counter}", $"Output {counter}", true);
            counter++;
            return testCase;
        }

        internal static Problem CreateFullProblem()
        {
            Problem problem = CreateNewProblem();
            for (int i = 0; i < 5; i++)
            {
                problem.AddTag(CreateNewTag());
                problem.AddTestCase(CreateNewTestCase());
            }
            return problem;
        }

        internal static ProblemList CreateNewProblemList()
        {
            ProblemList problemList = ProblemList.Create($"Problem List {counter}", $"Description {counter}", new());
            counter++;
            return problemList;
        }

        internal static ProblemList CreateFullProblemList()
        {
            ProblemList problemList = CreateNewProblemList();
            for (int i = 0; i < 5; i++)
            {
                problemList.AddProblem(CreateFullProblem());
            }
            return problemList;
        }

        internal static User CreateNewUser()
        {
            User user = User.Create($"User {counter}", $"user{counter}@example.com", Role.Student);
            counter++;
            return user;
        }

        internal static Language CreateNewLanguage()
        {
            Language lang = Language.Create($"lang {counter++}", "Lang", false, "", "", "RunCmd", "RunArgs", ".example");
            return lang;
        }

        internal static Submission CreateNewSubmission()
        {
            var problem = CreateNewProblem();
            var user = CreateNewUser();
            var lang = CreateNewLanguage();
            var submission = Submission.Create("code", lang, user, problem);
            return submission;
        }
        
        internal static SubmissionResult CreateNewSubmissionResult()
        {
            var submission = CreateNewSubmission();
            var result = SubmissionResult.Create(submission, new());
            return result;
        }

        internal static TestCaseResult CreateNewTestCaseResult()
        {
            var result = TestCaseResult.Create(new($"stdout{counter}", $"stderr{counter++}", 0, 1000, 64 * MB, ResultCode.SUCCESS));
            return result;
        }

        internal static Language CreateLanguage(string lang)
        {
            switch (lang)
            {
                case "python":
                    return Language.Create("python", "Python", false, "", "", "python.exe", "{SourceCodeFilePath}", ".py");
                case "c":
                    return Language.Create("c", "C", true, "gcc.exe", "-x c {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".c");
                case "cpp":
                    return Language.Create("cpp", "C++", true, "g++.exe", "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".cpp");
                case "rust":
                    return Language.Create("rust", "Rust", true, "rustc.exe", "{SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".rs");
                case "javascript":
                    return Language.Create("javascript", "JavaScript", false, "", "", "node.exe", "{SourceCodeFilePath}", ".js");
                case "java":
                    return Language.Create("java", "Java", true, "javac.exe", "{SourceCodeFilePath}", "java.exe", "main", ".java");
                case "go":
                    return Language.Create("go", "Go", true, "go.exe", "build {SourceCodeFilePath}", "{ExecutableFilePath}", "", ".go");
                default:
                    return null;
            }
        }
    }
}
