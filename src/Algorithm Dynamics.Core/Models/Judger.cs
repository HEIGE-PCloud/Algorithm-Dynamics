using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        /// <summary>
        /// Compile the source code in <see cref="_SourceCodeFilePath"/>.
        /// </summary>
        /// <param name="language">
        /// The <see cref="Language.CompileCommand"/> and 
        /// <see cref="Language.CompileArguments"/> are used for compilation.
        /// Note that the <see cref="Language.NeedCompile"/> is not checked.
        /// </param>
        /// <returns>The exit code of the compiler is returned.</returns>
        private async static Task<int> Compile(Language language)
        {
            // Clear old output
            _CompilationOutput = "";
            _CompilationError = "";

            // Set file extension
            string sourceFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);

            // Create compile process
            Process CompileProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand
                        .Replace("{SourceCodeFilePath}", sourceFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.CompileArguments
                        .Replace("{SourceCodeFilePath}", sourceFilePath)
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
        
        /// <summary>
        /// Execute the executable in <see cref="_ExecutableFilePath"/>.
        /// </summary>
        /// <param name="stdin">The standard input gets feeded to the program.</param>
        /// <param name="language">The language config contains the running config.</param>
        /// <param name="timeLimit">The time limit for the process.</param>
        /// <param name="memoryLimit">The memory limit for the process.</param>
        /// <returns></returns>
        private async static Task<int> Execute(string stdin, Language language, int timeLimit, long memoryLimit)
        {
            // Clear the old data
            _StandardOutput = "";
            _StandardError = "";
            _StatusCode = StatusCode.PENDING;
            _WorkingSet64 = 0;

            // Set file extension
            string sourceFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);

            // Create the execute process
            Process ExecuteProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.RunCommand
                        .Replace("{SourceCodeFilePath}", sourceFilePath)
                        .Replace("{ExecutableFilePath}", _ExecutableFilePath),
                    Arguments = language.RunArguments
                        .Replace("{SourceCodeFilePath}", sourceFilePath)
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
            ExecuteProcess.StandardInput.WriteLine(stdin);
            _StatusCode = StatusCode.RUNNING;

            // Start the time monitor
            Timer timer = new((e) =>
            {
                if (_StatusCode == StatusCode.RUNNING && ExecuteProcess.HasExited == false)
                {
                    ExecuteProcess.Kill();
                    _StatusCode = StatusCode.TIME_LIMIT_EXCEEDED;
                }
            }, null, timeLimit, Timeout.Infinite);

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
                    if (_WorkingSet64 > memoryLimit)
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
        
        /// <summary>
        /// Set the SourceCode File with its folder path and its name.
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="FileName"></param>
        public static void SetSourceCodeFilePath(string FolderPath, string FileName)
        {
            _SourceCodeFolderPath = FolderPath;
            _SourceCodeFilePath = Path.Combine(FolderPath, FileName) + "{SourceCodeFileExtension}";
            _ExecutableFilePath = Path.Combine(FolderPath, FileName) + ".exe";
        }

        /// <summary>
        /// Run the UserCode with the given data.
        /// Use the Progress for reporting progress.
        /// Use skipCompile if multiple data needs to run on the same code.
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Input"></param>
        /// <param name="language"></param>
        /// <param name="TimeLimit"></param>
        /// <param name="MemoryLimit"></param>
        /// <param name="Progress"></param>
        /// <param name="skipCompile"></param>
        /// <returns></returns>
        public async static Task<RunCodeResult> RunCode(string UserCode, string Input, Language language, int TimeLimit, long MemoryLimit, IProgress<int> Progress, bool skipCompile = false)
        {
            Progress.Report(0);

            // Add a \n to the end for the input
            if (string.IsNullOrEmpty(Input) == false && Input[^1] != '\n') Input += '\n';

            // Set file extension
            string sourceFilePath = _SourceCodeFilePath.Replace("{SourceCodeFileExtension}", language.FileExtension);

            // Save the code to the disk
            RunCodeResult result = new();
            Directory.CreateDirectory(_SourceCodeFolderPath);
            await File.WriteAllTextAsync(sourceFilePath, UserCode);

            // Compile if needed
            if (language.NeedCompile && (!skipCompile))
            {
                // Check if the compiler exists before compiling
                if (!ExistsOnPath(language.CompileCommand.Replace("{SourceCodeFilePath}", sourceFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath)))
                {
                    Progress.Report(100);
                    result.StandardError = $"The CompileCommand {language.CompileCommand} cannot be found.\nPlease check the programming language configuration.";
                    result.ResultCode = ResultCode.SYSTEM_ERROR;
                    return result;
                }

                // Compile and report any error
                if (await Compile(language) != 0)
                {
                    Progress.Report(100);
                    result.StandardOutput = _CompilationOutput;
                    result.StandardError = FilterErrorMessage(_CompilationError);
                    result.ResultCode = ResultCode.COMPILE_ERROR;
                    return result;
                }
            }
            // Finish compilation
            Progress.Report(10);

            // Check run command before running
            if (!ExistsOnPath(language.RunCommand.Replace("{SourceCodeFilePath}", sourceFilePath).Replace("{ExecutableFilePath}", _ExecutableFilePath)))
            {
                Progress.Report(100);
                result.StandardError 
                    = $"The RunCommand {language.RunCommand} cannot be found.\nPlease check the programming language configuration.";
                result.ResultCode = ResultCode.SYSTEM_ERROR;
                return result;
            }
            // Setup watch
            var watch = new Stopwatch();
            watch.Start();

            // Execute the code
            result.ExitCode = await Execute(Input, language, TimeLimit, MemoryLimit);
            
            // Save the results
            watch.Stop();
            result.StandardOutput = _StandardOutput;
            result.StandardError = FilterErrorMessage(_StandardError);
            result.CPUTime = watch.ElapsedMilliseconds;
            result.MemoryUsage = _WorkingSet64;

            // Report complete
            Progress.Report(100);

            // Handle TLE
            if (_StatusCode == StatusCode.TIME_LIMIT_EXCEEDED)
            {
                result.ResultCode = ResultCode.TIME_LIMIT_EXCEEDED;
                return result;
            }

            // Handle MLE
            if (_StatusCode == StatusCode.MEMORY_LIMIT_EXCEEDED)
            {
                result.ResultCode = ResultCode.MEMORY_LIMIT_EXCEEDED;
                return result;
            }

            // Handle RE
            if (!string.IsNullOrEmpty(result.StandardError) || result.ExitCode != 0)
            {
                result.ResultCode = ResultCode.RUNTIME_ERROR;
                return result;
            }

            // Success
            result.ResultCode = ResultCode.SUCCESS;
            return result;
        }

        /// <summary>
        /// Judge a TestCase
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="TestCase"></param>
        /// <param name="Language"></param>
        /// <param name="TimeLimit"></param>
        /// <param name="MemoryLimit"></param>
        /// <returns></returns>
        public async static Task<TestCaseResult> JudgeTestCase(string UserCode, TestCase TestCase, Language Language, int TimeLimit, long MemoryLimit, bool skipCompile = false)
        {
            RunCodeResult runCodeResult = await RunCode(UserCode, TestCase.Input, Language, TimeLimit, MemoryLimit, new Progress<int>(), skipCompile);
            
            // Handle WA
            if (runCodeResult.ResultCode == ResultCode.SUCCESS)
            {
                if (runCodeResult.StandardOutput.Trim() != TestCase.Output.Trim())
                {
                    runCodeResult.ResultCode = ResultCode.WRONG_ANSWER;
                }
            }

            // Not implemented edit result so must create it at the end
            TestCaseResult result = TestCaseResult.Create(runCodeResult);
            return result;
        }
        public async static Task<SubmissionResult> JudgeProblem(Submission Submission, IProgress<int> Progress)
        {
            // Create SubmissionResult
            SubmissionResult Result = SubmissionResult.Create(Submission, new());

            // Set up Judge Queue
            Queue<TestCase> JudgeQueue = new(Submission.Problem.TestCases);

            // Ready to judge, report progress
            Progress.Report(0);

            int testCasesCount = Submission.Problem.TestCases.Count;
            bool skipCompile = false;
            while (JudgeQueue.Count > 0)
            {
                TestCaseResult result = await JudgeTestCase(
                    Submission.Code,
                    JudgeQueue.Dequeue(),
                    Submission.Language,
                    Submission.Problem.TimeLimit,
                    Submission.Problem.MemoryLimit,
                    skipCompile);

                // Add result
                Result.AddTestCaseResult(result);

                // Report progress
                Progress.Report(100 * (testCasesCount - JudgeQueue.Count) / testCasesCount);

                // Skip compile
                skipCompile = true;
            }

            //Finish Judging
            Progress.Report(100);

            // Update problem status
            if (Result.ResultCode == ResultCode.SUCCESS)
            {
                Submission.Problem.Status = ProblemStatus.Solved;
            }
            else
            {
                if (Submission.Problem.Status == ProblemStatus.Todo)
                {
                    Submission.Problem.Status = ProblemStatus.Attempted;
                }
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
                Result.Add(await JudgeProblem(SubmissionQueue.Dequeue(), new Progress<int>()));
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

        private static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        private static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        private static string FilterErrorMessage(string error)
        {
            return error.Replace(_SourceCodeFolderPath, "").Trim('\\');
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