using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Algorithm_Dynamics.Core.Models;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class PlaygroundPage : Page
    {
        public PlaygroundPage()
        {
            InitializeComponent();
        }
        public string Code { get; set; }
        public string Input { get; set; }
        private const int DEFAULT_RUN_CODE_TIMELIMIT = 1000;
        private const int DEFAULT_RUN_CODE_MEMORYLIMIT = 64 * 1024 * 1024;
        private const string TIMELIMIT_KEY = "RunCodeTimeLimit";
        private const string MEMORYLIMIT_KEY = "RunCodeMemoryLimit";
        private int _timeLimit = DEFAULT_RUN_CODE_TIMELIMIT;
        private int _memoryLimit = DEFAULT_RUN_CODE_MEMORYLIMIT;

        private readonly ObservableCollection<Language> languages = new() { LanguageConfig.C, LanguageConfig.Cpp, LanguageConfig.Python, LanguageConfig.JavaScript, LanguageConfig.Rust, LanguageConfig.Go, LanguageConfig.Java};
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Read Run Code Time Limit and Memory Limit from the Setting
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            var CurrentTimeLimit = roamingSettings.Values[TIMELIMIT_KEY];
            if (CurrentTimeLimit != null)
            {
                _timeLimit = (int)CurrentTimeLimit;
            }
            else
            {
                roamingSettings.Values[TIMELIMIT_KEY] = DEFAULT_RUN_CODE_TIMELIMIT;
            }
            var CurrentMemoryLimit = roamingSettings.Values[MEMORYLIMIT_KEY];
            if (CurrentMemoryLimit != null)
            {
                _memoryLimit = (int)CurrentMemoryLimit;
            }
            else
            {
                roamingSettings.Values[MEMORYLIMIT_KEY] = DEFAULT_RUN_CODE_MEMORYLIMIT;
            }

            var progress = new Progress<int>(percent => { RunCodeProgressBar.Value = percent; });
            
            RunCodeButton.IsEnabled = false;

            RunCodeResult result = await Judger.RunCode(CodeEditor.Code, Input, languages[LanguageComboBox.SelectedIndex], _timeLimit, _memoryLimit, progress);
            
            RunCodeButton.IsEnabled = true;
            
            StatusTextBlock.Text = $"{result.ResultCode} Time: {result.CPUTime} ms Memory: {result.MemoryUsage / 1024 / 1024} MB";
            OutputBox.Text = result.StandardOutput + result.StandardError;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CodeEditor.Lang = LanguageComboBox.SelectedItem.ToString();
        }
    }
}
