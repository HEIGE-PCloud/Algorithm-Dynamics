using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Algorithm_Dynamics.Controls
{
    public sealed partial class CodeEditor : UserControl
    {
        public CodeEditor()
        {
            InitializeComponent();
            InitializeWebViewAsync();
            RequestedTheme = ElementTheme.Default;
        }

        /// <summary>
        /// Initialize the WebView control and load Editor.html
        /// </summary>
        async void InitializeWebViewAsync()
        {
            // Ensure the CoreWebView2 is loaded
            await WebView.EnsureCoreWebView2Async();
            WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            // Set the mapping
            StorageFolder AssetsDirectory = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "localeditor.algorithmdynamics.com",
                AssetsDirectory.Path,
                CoreWebView2HostResourceAccessKind.Allow
            );

            // Load Editor.html
            WebView.Source = new Uri("http://localeditor.algorithmdynamics.com/Editor.html");


            // Set default settings
            var editorConfig = new EditorConfig(GetTheme(), Lang, Code);
            await WebView.ExecuteScriptAsync($"window.config={JsonSerializer.Serialize(editorConfig)}");
        }

        /// <summary>
        /// Return the editor theme based on current requested theme
        /// </summary>
        /// <returns></returns>
        private string GetTheme()
        {
            if (RequestedTheme == ElementTheme.Dark) return "vs-dark";
            else if (RequestedTheme == ElementTheme.Light) return "vs";
            return "vs";
        }

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }
        
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(
                "Code",
                typeof(string),
                typeof(CodeEditor),
                new PropertyMetadata("")
            );

        public string Lang
        {
            get { return (string)GetValue(LangProperty); }
            set { SetValue(LangProperty, value); }
        }

        public static readonly DependencyProperty LangProperty =
            DependencyProperty.Register(
                "Lang",
                typeof(string),
                typeof(CodeEditor),
                new PropertyMetadata("")
            );

        private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string data = args.TryGetWebMessageAsString();
            Code = data;
        }

        public class EditorConfig
        {
            public EditorConfig(string theme, string language, string code)
            {
                Theme = theme;
                Language = language;
                Code = code;
            }
#nullable enable
            public string? Theme { get; set; } = null;
            public string? Language { get; set; } = null;
            public string? Code { get; set; } = null;
#nullable restore
        }
    }
}
