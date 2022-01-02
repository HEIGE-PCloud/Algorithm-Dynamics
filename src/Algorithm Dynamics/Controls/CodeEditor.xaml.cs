using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Algorithm_Dynamics.Controls
{
    public sealed partial class CodeEditor : UserControl
    {
        public CodeEditor()
        {
            InitializeComponent();
            InitializeWebViewAsync();
            Code = "qwqwqwq";
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
                new PropertyMetadata(null)
            );
        
        private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string data = args.TryGetWebMessageAsString();
            Code = data;
        }
    }
}
