using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Text.Json;
using CommunityToolkit.WinUI.UI.Helpers;

namespace Algorithm_Dynamics.Controls
{
    public sealed partial class CodeEditor : UserControl
    {
        private readonly ThemeListener themeListener = new();
        public CodeEditor()
        {
            InitializeComponent();
            InitializeWebViewAsync();
            themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            Unloaded += CodeEditor_Unloaded;
        }

        private void CodeEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            // Close the WebView when unloaded
            // https://github.com/microsoft/microsoft-ui-xaml/issues/4752
            WebView.Close();
        }

        /// <summary>
        /// Not working yet
        /// </summary>
        /// <param name="sender"></param>
        private void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            UpdateEditorConfig(new EditorConfig(GetTheme(RequestedTheme), null, null));
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

        /// <summary>
        /// Process the initialization process
        /// Update the Code when receive the value send by the Monaco Editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string data = args.TryGetWebMessageAsString();
            if (data == "[Status] Request Configuration")
            {
                EditorConfig editorConfig = new (GetTheme(RequestedTheme), Lang, Code);
                await WebView.ExecuteScriptAsync($"window.config={JsonSerializer.Serialize(editorConfig)}");
                WebView.CoreWebView2.PostWebMessageAsString("Configuration Sent");
            }
            else if (data == "[Status] Ready")
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                WebView.Visibility = Visibility.Visible;
            } 
            else
            {
                // [Data] actual code
                _code = data.Substring("[Data] ".Length, data.Length - "[Data] ".Length);
                return;
            }
        }

        /// <summary>
        /// Return the editor theme based on current requested theme
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        private string GetTheme(ElementTheme theme)
        {
            if (theme == ElementTheme.Dark) return "vs-dark";
            else if (theme == ElementTheme.Light) return "vs";
            else if (theme == ElementTheme.Default)
            {
                MainWindow m_window = App.m_window;
                if (m_window.Content is FrameworkElement rootElement)
                {
                    if (rootElement.RequestedTheme != ElementTheme.Default)
                        return GetTheme(rootElement.RequestedTheme);
                    else
                    {
                        if (App.Current.RequestedTheme == ApplicationTheme.Dark)
                            return "vs-dark";
                        else
                            return "vs";
                    }
                }
            }
            return "vs";
        }

        /// <summary>
        /// Update the editor config of the Monaco Editor
        /// </summary>
        /// <param name="editorConfig"></param>
        private void UpdateEditorConfig(EditorConfig editorConfig)
        {
            WebView.CoreWebView2?.PostWebMessageAsJson(JsonSerializer.Serialize(editorConfig));
        }

        private string _code;
        public string Code
        {
            get => _code; 
            set
            {
                if (value != _code)
                {
                    _code = value;
                    UpdateEditorConfig(new EditorConfig(null, null, value));
                    SetValue(CodeProperty, value);
                }
            }
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
            set 
            {
                UpdateEditorConfig(new EditorConfig(null, value, null));
                SetValue(LangProperty, value);
            }
        }

        public static readonly DependencyProperty LangProperty =
            DependencyProperty.Register(
                "Lang",
                typeof(string),
                typeof(CodeEditor),
                new PropertyMetadata("")
            );

        public new ElementTheme RequestedTheme
        {
            get { return base.RequestedTheme; }
            set
            {
                UpdateEditorConfig(new EditorConfig(GetTheme(value), null, null));
                base.RequestedTheme = value;
            }
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
