﻿using System;
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
        public static string SourceCodeFilePath { get; set; }
        public static string SourceCodeFolderPath { get; set; }
        public static string ExecutableFilePath { get; set; }
        private static string _StandardOutput;
        private static string _StandardError;
        private static string _CompilationOutput;
        private static string _CompilationError;
        private async static Task<int> Compile(Language language)
        {
            Process CompileProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.CompileCommand.Replace("{SourceCodeFilePath}", SourceCodeFilePath).Replace("{ExecutableFilePath}", ExecutableFilePath),
                    Arguments = language.CompileArguments.Replace("{SourceCodeFilePath}", SourceCodeFilePath).Replace("{ExecutableFilePath}", ExecutableFilePath),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                },
                EnableRaisingEvents = true
            };
            // Timer CompileTimer = new Timer(delegate { CompileProcess.Kill(); }, null, 10000, Timeout.Infinite);
            CompileProcess.OutputDataReceived += new DataReceivedEventHandler(CompileProcess_OutputDataReceived);
            CompileProcess.ErrorDataReceived += new DataReceivedEventHandler(CompileProcess_ErrorDataReceived);
            CompileProcess.Start();
            CompileProcess.BeginOutputReadLine();
            CompileProcess.BeginErrorReadLine();
            await CompileProcess.WaitForExitAsync();
            return CompileProcess.ExitCode;
        }
        private async static Task<int> Execute(string Input, Language language)
        {
            Process ExecuteProcess = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = language.RunCommand.Replace("{SourceCodeFilePath}", SourceCodeFilePath).Replace("{ExecutableFilePath}", ExecutableFilePath),
                    Arguments = language.RunArguments.Replace("{SourceCodeFilePath}", SourceCodeFilePath).Replace("{ExecutableFilePath}", ExecutableFilePath),
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
            Timer timer = new Timer(delegate { ExecuteProcess.Kill(); }, null, 2000, Timeout.Infinite);
            ExecuteProcess.Start();
            ExecuteProcess.BeginOutputReadLine();
            ExecuteProcess.BeginErrorReadLine();
            ExecuteProcess.StandardInput.WriteLine(Input);
            await ExecuteProcess.WaitForExitAsync();
            ExecuteProcess.WaitForExit();
            return ExecuteProcess.ExitCode;
        }
        public static void SetSourceCodeFilePath(string FolderPath, string FileName)
        {
            SourceCodeFolderPath = FolderPath;
            SourceCodeFilePath = Path.Combine(FolderPath, FileName) + ".txt";
            ExecutableFilePath = Path.Combine(FolderPath, FileName) + ".exe";
        }
        public async static Task<SubmissionResult> RunCode(string UserCode, string Input, Language language)
        {
            clear();
            SubmissionResult result = new();
            await File.WriteAllTextAsync(SourceCodeFilePath, UserCode);
            if (language.NeedCompile)
            {
                if (await Compile(language) != 0)
                {
                    result.StandardOutput = _CompilationOutput;
                    result.StandardError = _CompilationError;
                    result.resultCode = SubmissionResult.ResultCode.COMPILE_ERROR;
                    return result;
                }
            }
            int exitCode = await Execute(Input, language);
            result.StandardOutput = _StandardOutput;
            result.StandardError = _StandardError;
            if (exitCode == 0)
            {
                result.resultCode = SubmissionResult.ResultCode.SUCCESS;
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
        private static void clear()
        {
            _StandardOutput = "";
            _StandardError = "";
            _CompilationOutput = "";
            _CompilationError = "";
        }
    }
}