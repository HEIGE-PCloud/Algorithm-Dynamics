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
        public async static Task<TestCaseResult> RunCode(string UserCode, string Input, Language language, int TimeLimit)
        {
            TestCaseResult result = new();
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
            int exitCode = await Execute(Input, language, TimeLimit);
            watch.Stop();
            result.StandardOutput = _StandardOutput;
            result.StandardError = _StandardError;
            result.CPUTime = (int)watch.ElapsedMilliseconds;
            if (!string.IsNullOrEmpty(result.StandardError))
            {
                result.resultCode = ResultCode.RUNTIME_ERROR;
                return result;
            }
            if (_StatusCode == StatusCode.TIME_LIMIT_EXCEEDED)
            {
                result.resultCode = ResultCode.TIME_LIMIT_EXCEEDED;
                return result;
            }
            if (exitCode == 0)
            {
                result.resultCode = ResultCode.SUCCESS;
            }
            return result;
        }
        public async static Task<TestCaseResult> JudgeTestCase(string UserCode, TestCase TestCase, Language Language, int TimeLimit)
        {
            TestCaseResult result = await RunCode(UserCode, TestCase.Input, Language, TimeLimit);
            if (result.resultCode == ResultCode.SUCCESS)
            {
                if (result.StandardOutput.Trim() != TestCase.Output.Trim())
                {
                    result.resultCode = ResultCode.WRONG_ANSWER;
                }
            }
            return result;
        }
        public async static Task<SubmissionResult> JudgeProblem(Submission Submission, Language Language)
        {
            SubmissionResult result = new();
            result.Submission = Submission;
            Queue<TestCase> JudgeQueue = new(Submission.Problem.TestCases);
            while (JudgeQueue.Count > 0)
            {
                result.TestCaseResults.Append(await JudgeTestCase(Submission.Code, JudgeQueue.Dequeue(), Language, Submission.Problem.TimeLimit));
            }
            return result;
        }
        public async static Task<AssignmentSubmissionResult> JudgeAssignment(Assignment Assignment, AssignmentSubmission AssignmentSubmission, Language Language)
        {
            AssignmentSubmissionResult result = new();
            result.AssignmentSubmission = AssignmentSubmission;
            Queue<Submission> SubmissionQueue = new(AssignmentSubmission.Submissions);
            while (SubmissionQueue.Count > 0)
            {
                result.SubmissionResults.Append(await JudgeProblem(SubmissionQueue.Dequeue(), Language));
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