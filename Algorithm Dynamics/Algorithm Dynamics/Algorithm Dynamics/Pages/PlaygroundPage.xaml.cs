using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Text.Json;
using System.Diagnostics;
using System.Threading;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaygroundPage : Page
    {
        public string Code { get; set; }
        public StringBuilder StandardOutput { get; set; } = new StringBuilder("");
        public StringBuilder StandardError { get; set; } = new StringBuilder("");

        public PlaygroundPage()
        {
            this.InitializeComponent();
            InitializeAsync();
        }
        async void InitializeAsync()
        {
            await myWebView.EnsureCoreWebView2Async();
            myWebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            Windows.Storage.StorageFolder AssetsDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            myWebView.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets.example", AssetsDirectory.Path, CoreWebView2HostResourceAccessKind.Allow);
            myWebView.Source = new Uri("http://appassets.example/Editor.html");
            //await myWebView.ExecuteScriptAsync($"window.config={JsonSerializer.Serialize(editorSetting)}");
        }
        private async void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string data = args.TryGetWebMessageAsString();
            if (data == "init")
            {
                EditorSetting editorSetting = new EditorSetting();
                await myWebView.ExecuteScriptAsync($"window.config={JsonSerializer.Serialize(editorSetting)}");
                myWebView.CoreWebView2.PostWebMessageAsString("init");
            }
            else if (data == "loaded")
            {
                myWebView.Visibility = Visibility.Visible;
            }
            else
            {
                Code = data;
            }
        }
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            RunCodeButton.IsEnabled = false;
            RunCodeProgressRing.IsActive = true;
            OutputTextBlock.Text = "";
            ErrorTextBlock.Text = "";
            StandardOutput.Clear();
            StandardError.Clear();
            Process proc = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    ArgumentList = { "-c", $"{Code}" },
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
            proc.StandardInput.WriteLine(InputTextBlock.Text);
            await proc.WaitForExitAsync();
            proc.WaitForExit();
            RunCodeButton.IsEnabled = true;
            RunCodeProgressRing.IsActive = false;
            //if (!proc.WaitForExit(1000))
            //{
            //    proc.Kill();
            //    ErrorTextBlock.Text = "Time Limit Exceed";
            //    ResultTextBlock.Text = "TLE";
            //}
            OutputTextBlock.Text = StandardOutput.ToString();
            ErrorTextBlock.Text = StandardError.ToString();
            if (string.IsNullOrEmpty(ErrorTextBlock.Text))
            {
                CodeExecutePivot.SelectedItem = OutputPanel;
                ResultTextBlock.Text = "Passed";
            }
            else
            {
                CodeExecutePivot.SelectedItem = ErrorPanel;
                ResultTextBlock.Text = "CE";
            }
        }
        private void Proc_Exited(object sender, EventArgs e)
        {

        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                StandardOutput.Append(e.Data + '\n');
            }
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                StandardError.Append(e.Data + '\n');
            }
        }

        private async void DebugButton_Click(object sender, RoutedEventArgs e)
        {
            EditorSetting editorSetting = new EditorSetting();
            await myWebView.ExecuteScriptAsync($"window.config={JsonSerializer.Serialize(editorSetting)}");
        }
    }
    public class EditorSetting
    {
        public string theme { get; set; }
        public string language { get; set; }
        public string value { get; set; }
        public EditorSetting()
        {
            theme = "vs-dark";
            language = "python";
            value = "print(\"Hello world\")";
        }
    }
}
