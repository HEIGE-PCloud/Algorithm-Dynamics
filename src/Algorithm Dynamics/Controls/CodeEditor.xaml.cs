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
        }

        /// <summary>
        /// Initialize the WebView control and load Editor.html
        /// </summary>
        async void InitializeWebViewAsync()
        {
            // Ensure the CoreWebView2 is loaded
            await WebView.EnsureCoreWebView2Async();

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
    }
}
