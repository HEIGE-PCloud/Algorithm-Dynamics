using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Judger
    {
        private static string StandardOutput;
        private static string StandardError;
        public async static Task<SubmissionResult> RunCode(string UserCode, string Input)
        {
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
                    CreateNoWindow = true
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
            result.StandardOutput = StandardOutput;
            result.StandardError = StandardError;
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
                StandardOutput += e.Data + '\n';
            }
        }

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                StandardError += e.Data + '\n';
            }
        }
        private static void clear()
        {
            StandardOutput = "";
            StandardError = "";
        }
    }
}
