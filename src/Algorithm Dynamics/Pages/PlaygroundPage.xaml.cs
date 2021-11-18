using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// PlaygroundPage
    /// </summary>
    public sealed partial class PlaygroundPage : Page
    {
        public string Code { get; set; }
        public StringBuilder StandardOutput { get; set; } = new StringBuilder("");
        public StringBuilder StandardError { get; set; } = new StringBuilder("");
        
        public PlaygroundPage()
        {
            InitializeComponent();
            InitializeAsync();
        }
        async void InitializeAsync()
        {
            await myWebView.EnsureCoreWebView2Async();
            myWebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            Windows.Storage.StorageFolder AssetsDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            myWebView.CoreWebView2.SetVirtualHostNameToFolderMapping("localeditor.algorithmdynamics.com", AssetsDirectory.Path, CoreWebView2HostResourceAccessKind.Allow);
            myWebView.Source = new Uri("http://localeditor.algorithmdynamics.com/Editor.html");
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
            // Disable UI Elements
            RunCodeButton.IsEnabled = false;
            RunCodeProgressRing.IsActive = true;
            
            // Empty Legacy outputs
            OutputTextBlock.Text = "";
            ErrorTextBlock.Text = "";
            StandardOutput.Clear();
            StandardError.Clear();;

            TestCaseResult result = await Judger.RunCode(Code, InputTextBlock.Text, LanguageConfig.Cpp, 2000);

            // Enable UI Elemnts
            RunCodeButton.IsEnabled = true;
            RunCodeProgressRing.IsActive = false;
            // Update output
            OutputTextBlock.Text = result.StandardOutput;
            ErrorTextBlock.Text = result.StandardError;
            ResultTextBlock.Text = result.resultCode.ToString();
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
            language = "cpp";
            value = @"#include <iostream>
using namespace std;
int main()
{
    int a, b;
    cin >> a >> b;
    cout << a + b << endl;
    return 0;
}";
        }
    }
}
