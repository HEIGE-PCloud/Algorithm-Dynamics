using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Algorithm_Dynamics.Core.Models
{
    public class Judger
    {
        private static string _StandardOutput;
        private static string _StandardError;
        private static LanguageConfig _PythonLanguageConfig = new(false, "python", new Collection<string>{"1"});
        private static Dictionary<Language, LanguageConfig> _LanguageConfigDictionary = new()
        {
            {Language.Python, _PythonLanguageConfig}
        };
        public async static Task<SubmissionResult> RunCode(string UserCode, string Input, Language language)
        {
            var languageConfig = _LanguageConfigDictionary[language];
            if (languageConfig.NeedCompile)
            {
                // Write UserCode to file
                // var fileName = $"{Guid.NewGuid()}.{languageConfig.FileExtension}";
                // var filePath = $"{languageConfig.FilePath}{fileName}";
                // FileHelper.WriteFile(filePath, UserCode);
                // Compile
                Process CompileProcess = new()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = languageConfig.CompileCommand,
                        Arguments = languageConfig.CompileArguments,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                // Timer CompileTimer = new Timer(delegate { CompileProcess.Kill(); }, null, 10000, Timeout.Infinite);
                CompileProcess.Start();
                await CompileProcess.WaitForExitAsync();
            }
            // clear _standardInput and _standardOutput
            clear();
            SubmissionResult result = new();
            Process proc = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    ArgumentList = { "-c", $"{UserCode}" },
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };
            proc.OutputDataReceived += new DataReceivedEventHandler(Proc_OutputDataReceived);
            proc.ErrorDataReceived += new DataReceivedEventHandler(Proc_ErrorDataReceived);
            proc.Exited += new EventHandler(Proc_Exited);
            Timer timer = new Timer(delegate { proc.Kill(); }, null, 2000, Timeout.Infinite);
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.StandardInput.WriteLine(Input);
            await proc.WaitForExitAsync();
            proc.WaitForExit();
            result.StandardOutput = _StandardOutput;
            result.StandardError = _StandardError;
            result.UserResultCode = SubmissionResult.ResultCode.SUCCESS;
            return result;
        }
        private static void Proc_Exited(object sender, EventArgs e)
        {

        }
        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _StandardOutput += e.Data + '\n';
            }
        }

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
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
        }
        private class LanguageConfig
        {
            public bool NeedCompile = false;
            public string CompileCommand = "";
            public string RunCommand = "";
            public string FileName = "";
            public string FileExtension = "";
            public string CompileArguments = "";
            public Collection<string> RunArguments = new();
            public LanguageConfig(bool needCompile, string compileCommand, string runCommand)
            {
                NeedCompile = needCompile;
                CompileCommand = compileCommand;
                RunCommand = runCommand;
            }
            public LanguageConfig(bool needCompile, string fileName, Collection<string> arguments)
            {
                NeedCompile = needCompile;
                FileName = fileName;
                Arguments = arguments;
            }
        }

    }
}
