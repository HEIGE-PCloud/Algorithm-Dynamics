using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
namespace Algorithm_Dynamics.Core.Models
{
    public static class Judger
    {
        private static string _SourceCodeFilePath;
        private static string _SourceCodeFolderPath;
        private static string _ExecutableFilePath;
        private static string _StandardOutput;
        private static string _StandardError;
        private static string _CompilationOutput;
        private static string _CompilationError;
        private static StatusCode _StatusCode;
        private async static Task<int> Compile(Language language)
        {
            _CompilationOutput = "";
            _CompilationError = "";
            Process CompileProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand.Replace("{SourceCodeFilePath}", _SourceCodeFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.CompileArguments.Replace("{SourceCodeFilePath}", _SourceCodeFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true
            };
            CompileProcess.OutputDataReceived += new DataReceivedEventHandler(CompileProcess_OutputDataReceived);
            CompileProcess.ErrorDataReceived += new DataReceivedEventHandler(CompileProcess_ErrorDataReceived);
            CompileProcess.Start();
            CompileProcess.BeginOutputReadLine();
            CompileProcess.BeginErrorReadLine();
            await CompileProcess.WaitForExitAsync();
            return CompileProcess.ExitCode;
        }
        private async static Task<int> Execute(string Input, Language language, int TimeLimit)
        {
            _StandardOutput = "";
            _StandardError = "";
            _StatusCode = StatusCode.PENDING;
            Process ExecuteProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.RunCommand.Replace("{SourceCodeFilePath}", _SourceCodeFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.RunArguments.Replace("{SourceCodeFilePath}", _SourceCodeFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };
            ExecuteProcess.OutputDataReceived += new DataReceivedEventHandler(ExecuteProcess_OutputDataReceived);
            ExecuteProcess.ErrorDataReceived += new DataReceivedEventHandler(ExecuteProcess_ErrorDataReceived);
            ExecuteProcess.Exited += new EventHandler(ExecuteProcess_Exited);
            Timer timer = new Timer(delegate 
            {
            if (_StatusCode == StatusCode.RUNNING)
                {
                    ExecuteProcess.Kill();
                    _StatusCode = StatusCode.TIME_LIMIT_EXCEEDED;
                }
            }, null, TimeLimit, Timeout.Infinite);
            _StatusCode = StatusCode.RUNNING;
            ExecuteProcess.Start();
            ExecuteProcess.BeginOutputReadLine();
            ExecuteProcess.BeginErrorReadLine();
            ExecuteProcess.StandardInput.WriteLine(Input);
            await ExecuteProcess.WaitForExitAsync();
            return ExecuteProcess.ExitCode;
        }
        public static void SetSourceCodeFilePath(string FolderPath, string FileName)
        {
            _SourceCodeFolderPath = FolderPath;
            _SourceCodeFilePath = Path.Combine(FolderPath, FileName) + ".txt";
            _ExecutableFilePath = Path.Combine(FolderPath, FileName) + ".exe";
        }
        public async static Task<TestCaseResult> RunCode(string UserCode, TestCase testCase, Language language, int TimeLimit)
        {
            TestCaseResult result = new();
            result.TestCase = testCase;
            Directory.CreateDirectory(_SourceCodeFolderPath);
            await File.WriteAllTextAsync(_SourceCodeFilePath, UserCode);
            if (language.NeedCompile)
            {
                if (await Compile(language) != 0)
                {
                    result.StandardOutput = _CompilationOutput;
                    result.StandardError = _CompilationError;
                    result.resultCode = ResultCode.COMPILE_ERROR;
                    return result;
                }
            }
            var watch = new Stopwatch();
            watch.Start();
            int exitCode = await Execute(testCase.Input, language, TimeLimit);
            watch.Stop();
            result.StandardOutput = _StandardOutput;
            result.StandardError = _StandardError;
            result.CPUTime = (int)watch.ElapsedMilliseconds;
            if (exitCode == 0)
            {
                result.resultCode = ResultCode.SUCCESS;
            }
            return result;
        }
        public async static Task<SubmissionResult> JudgeProblem(Problem problem, Submission submission, Language language)
        {
            SubmissionResult result = new();
            result.Submission = submission;
            Queue<TestCase> JudgeQueue = new();
            foreach (var TestCase in problem.TestCases)
            {
                JudgeQueue.Enqueue(TestCase);
            }
            while (JudgeQueue.Count > 0)
            {
                result.TestCaseResults.Append(await RunCode(submission.Code, JudgeQueue.Dequeue(), language, problem.TimeLimit));
            }
            return result;
        }
        public async static Task<AssignmentSubmissionResult> JudgeAssignment(Assignment assignment, AssignmentSubmission assignmentSubmission, Language language)
        {
            AssignmentSubmissionResult result = new();
            result.AssignmentSubmission = assignmentSubmission;
            Queue<Problem> ProblemQueue = new();
            Queue<Submission> SubmissionQueue = new();
            foreach (var Problem in assignment)
            {
                ProblemQueue.Enqueue(Problem);
            }
            foreach (var Submission in assignmentSubmission.Submissions)
            {
                SubmissionQueue.Enqueue(Submission);
            }
            while (ProblemQueue.Count > 0 && SubmissionQueue.Count > 0)
            {
                result.SubmissionResults.Append(await JudgeProblem(ProblemQueue.Dequeue(), SubmissionQueue.Dequeue(), language));
            }
            return result;
        }
        private static void CompileProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _CompilationError += e.Data + '\n';
            }
        }

        private static void CompileProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _CompilationOutput += e.Data + '\n';
            }
        }
        private static void ExecuteProcess_Exited(object sender, EventArgs e)
        {
            if (_StatusCode != StatusCode.TIME_LIMIT_EXCEEDED)
            {
                _StatusCode = StatusCode.FINISHED;
            }
        }
        private static void ExecuteProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _StandardOutput += e.Data + '\n';
            }
        }

        private static void ExecuteProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _StandardError += e.Data + '\n';
            }
        }
    }
    public enum StatusCode
    {
        PENDING,
        RUNNING,
        FINISHED,
        TIME_LIMIT_EXCEEDED
    }
}