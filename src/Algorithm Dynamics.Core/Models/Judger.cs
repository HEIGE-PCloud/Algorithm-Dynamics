using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
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
        private static long _WorkingSet64;
        private static StatusCode _StatusCode;
        private async static Task<int> Compile(Language language)
        {
            // Clear old output
            _CompilationOutput = "";
            _CompilationError = "";

            // Set file extension
            _SourceCodeFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);

            // Create compile process
            Process CompileProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand
                        .Replace("{SourceCodeFilePath}", _SourceCodeFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.CompileArguments
                        .Replace("{SourceCodeFilePath}", _SourceCodeFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true
            };

            // Bind event handler
            CompileProcess.OutputDataReceived +=
                new DataReceivedEventHandler(CompileProcess_OutputDataReceived);
            CompileProcess.ErrorDataReceived +=
                new DataReceivedEventHandler(CompileProcess_ErrorDataReceived);

            // Start the process
            CompileProcess.Start();
            CompileProcess.BeginOutputReadLine();
            CompileProcess.BeginErrorReadLine();

            // Wait for exit
            await CompileProcess.WaitForExitAsync();
            return CompileProcess.ExitCode;
        }
        private async static Task<int> Execute(string Input, Language language, int TimeLimit, long MemoryLimit)
        {
            // Clear the old data
            _StandardOutput = "";
            _StandardError = "";
            _StatusCode = StatusCode.PENDING;
            _WorkingSet64 = 0;

            // Set file extension
            _SourceCodeFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);

            // Create the execute process
            Process ExecuteProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.RunCommand
                        .Replace("{SourceCodeFilePath}", _SourceCodeFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.RunArguments
                        .Replace("{SourceCodeFilePath}", _SourceCodeFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };

            // Bind the event handler
            ExecuteProcess.OutputDataReceived +=
                new DataReceivedEventHandler(ExecuteProcess_OutputDataReceived);
            ExecuteProcess.ErrorDataReceived +=
                new DataReceivedEventHandler(ExecuteProcess_ErrorDataReceived);
            ExecuteProcess.Exited +=
                new EventHandler(ExecuteProcess_Exited);

            // Start running
            ExecuteProcess.Start();
            ExecuteProcess.BeginOutputReadLine();
            ExecuteProcess.BeginErrorReadLine();
            ExecuteProcess.StandardInput.WriteLine(Input);
            _StatusCode = StatusCode.RUNNING;

            // Start the time monitor
            Timer timer = new(delegate
            {
                if (_StatusCode == StatusCode.RUNNING && ExecuteProcess.HasExited == false)
                {
                    ExecuteProcess.Kill();
                    _StatusCode = StatusCode.TIME_LIMIT_EXCEEDED;
                }
            }, null, TimeLimit, Timeout.Infinite);

            // Create the memory monitor
            Thread MemoryMonitor = new(() =>
            {
                while (ExecuteProcess.HasExited == false)
                {
                    ExecuteProcess.Refresh();
                    try
                    {
                        _WorkingSet64 = ExecuteProcess.PeakWorkingSet64;
                    }
                    catch (InvalidOperationException) { break; }
                    if (_WorkingSet64 > MemoryLimit)
                    {
                        ExecuteProcess.Kill();
                        _StatusCode = StatusCode.MEMORY_LIMIT_EXCEEDED;
                    }
                }
            });
            MemoryMonitor.Start();

            // Wait for exit
            await ExecuteProcess.WaitForExitAsync();
            return ExecuteProcess.ExitCode;
        }
        public static void SetSourceCodeFilePath(string FolderPath, string FileName)
        {
            _SourceCodeFolderPath = FolderPath;
            _SourceCodeFilePath = Path.Combine(FolderPath, FileName) + "{SourceCodeFileExtension}";
            _ExecutableFilePath = Path.Combine(FolderPath, FileName) + ".exe";
        }
        public async static Task<RunCodeResult> RunCode(string UserCode, string Input, Language language, int TimeLimit, long MemoryLimit, IProgress<int> Progress)
        {
            Progress.Report(0);
            if (Input[Input.Length - 1] != '\n') Input += '\n';
            // Set file extension
            _SourceCodeFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);
            RunCodeResult result = new();
            Directory.CreateDirectory(_SourceCodeFolderPath);
            await File.WriteAllTextAsync(_SourceCodeFilePath, UserCode);
            if (language.NeedCompile)
            {
                if (await Compile(language) != 0)
                {
                    Progress.Report(100);
                    result.StandardOutput = _CompilationOutput;
                    result.StandardError = _CompilationError;
                    result.ResultCode = ResultCode.COMPILE_ERROR;
                    return result;
                }
            }
            Progress.Report(50);
            var watch = new Stopwatch();
            watch.Start();
            result.ExitCode = await Execute(Input, language, TimeLimit, MemoryLimit);
            watch.Stop();
            result.StandardOutput = _StandardOutput;
            result.StandardError = _StandardError;
            result.CPUTime = watch.ElapsedMilliseconds;
            result.MemoryUsage = _WorkingSet64;
            Progress.Report(100);
            if (_StatusCode == StatusCode.TIME_LIMIT_EXCEEDED)
            {
                result.ResultCode = ResultCode.TIME_LIMIT_EXCEEDED;
                return result;
            }
            if (_StatusCode == StatusCode.MEMORY_LIMIT_EXCEEDED)
            {
                result.ResultCode = ResultCode.MEMORY_LIMIT_EXCEEDED;
                return result;
            }
            if (!string.IsNullOrEmpty(result.StandardError)
                || result.ExitCode != 0)
            {
                result.ResultCode = ResultCode.RUNTIME_ERROR;
                return result;
            }
            result.ResultCode = ResultCode.SUCCESS;
            return result;
        }
        public async static Task<TestCaseResult> JudgeTestCase(string UserCode, TestCase TestCase, Language Language, int TimeLimit, long MemoryLimit)
        {
            TestCaseResult result = new(TestCase, await RunCode(UserCode, TestCase.Input, Language, TimeLimit, MemoryLimit, new Progress<int>()));
            if (result.ResultCode == ResultCode.SUCCESS)
            {
                if (result.StandardOutput.Trim() != TestCase.Output.Trim())
                {
                    result.ResultCode = ResultCode.WRONG_ANSWER;
                }
            }
            return result;
        }
        public async static Task<SubmissionResult> JudgeProblem(Submission Submission)
        {
            SubmissionResult Result = new();
            Result.Submission = Submission;
            Queue<TestCase> JudgeQueue = new(Submission.Problem.TestCases);
            while (JudgeQueue.Count > 0)
            {
                Result.Add(
                    await JudgeTestCase(
                        Submission.Code,
                        JudgeQueue.Dequeue(),
                        Submission.Language,
                        Submission.Problem.TimeLimit,
                        Submission.Problem.MemoryLimit
                    )
                );
            }
            return Result;
        }
        public async static Task<AssignmentSubmissionResult> JudgeAssignment(AssignmentSubmission AssignmentSubmission)
        {
            AssignmentSubmissionResult Result = new();
            Result.AssignmentSubmission = AssignmentSubmission;
            Queue<Submission> SubmissionQueue = new(AssignmentSubmission.Submissions);
            while (SubmissionQueue.Count > 0)
            {
                Result.Add(await JudgeProblem(SubmissionQueue.Dequeue()));
            }
            return Result;
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
            if (_StatusCode != StatusCode.TIME_LIMIT_EXCEEDED
                && _StatusCode != StatusCode.MEMORY_LIMIT_EXCEEDED)
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
        TIME_LIMIT_EXCEEDED,
        MEMORY_LIMIT_EXCEEDED
    }
}