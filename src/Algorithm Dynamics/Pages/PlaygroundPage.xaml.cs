using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
            Core.Models.Language.All.ForEach(lang => Languages.Add(lang));
        }

        private string Input = "";
        private const int ms = 1000;
        private const int MB = 1024 * 1024;
        private const int DEFAULT_RUN_CODE_TIMELIMIT = 1 * ms;
        private const int DEFAULT_RUN_CODE_MEMORYLIMIT = 64 * MB;
        private const string TIMELIMIT_KEY = "RunCodeTimeLimit";
        private const string MEMORYLIMIT_KEY = "RunCodeMemoryLimit";
        private int _timeLimit = DEFAULT_RUN_CODE_TIMELIMIT;
        private int _memoryLimit = DEFAULT_RUN_CODE_MEMORYLIMIT;
        private readonly ObservableCollection<Language> Languages = new();
        
        /// <summary>
        /// RunCode using <see cref="Judger.RunCode"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings 
                = ApplicationData.Current.LocalSettings;
            // Read RunCode TimeLimit from settings
            var CurrentTimeLimit = localSettings.Values[TIMELIMIT_KEY];
            if (CurrentTimeLimit != null)
            {
                _timeLimit = (int)CurrentTimeLimit;
            }
            else
            {
                localSettings.Values[TIMELIMIT_KEY] 
                    = DEFAULT_RUN_CODE_TIMELIMIT;
            }

            // Read RunCode MemoryLimit from settings
            var CurrentMemoryLimit = localSettings.Values[MEMORYLIMIT_KEY];
            if (CurrentMemoryLimit != null)
            {
                _memoryLimit = (int)CurrentMemoryLimit;
            }
            else
            {
                localSettings.Values[MEMORYLIMIT_KEY] 
                    = DEFAULT_RUN_CODE_MEMORYLIMIT;
            }

            // Set the progress bar and button
            var progress = new Progress<int>(percent 
                => { RunCodeProgressBar.Value = percent; });
            RunCodeButton.IsEnabled = false;

            // Run Code
            RunCodeResult result = await Judger.RunCode(
                CodeEditor.Code,
                Input,
                Languages[LanguageComboBox.SelectedIndex],
                _timeLimit,
                _memoryLimit,
                progress);

            // Restore the button and display result
            RunCodeButton.IsEnabled = true;
            StatusTextBlock.Text 
                = $"{result.Result} Time: {result.CPUTime} ms Memory: {result.MemoryUsage / MB} MB";
            OutputBox.Text = result.StandardOutput + result.StandardError;
        }

        /// <summary>
        /// Set CodeEditor language based on the selected language config
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Language language = LanguageComboBox.SelectedItem as Language;
            CodeEditor.Lang = language.Name;
        }
    }
}
