﻿using Algorithm_Dynamics.Core.Models;
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
        public string Code { get; set; }
        public string Input { get; set; }
        private const int DEFAULT_RUN_CODE_TIMELIMIT = 1000;
        private const int DEFAULT_RUN_CODE_MEMORYLIMIT = 64 * 1024 * 1024;
        private const string TIMELIMIT_KEY = "RunCodeTimeLimit";
        private const string MEMORYLIMIT_KEY = "RunCodeMemoryLimit";
        private int _timeLimit = DEFAULT_RUN_CODE_TIMELIMIT;
        private int _memoryLimit = DEFAULT_RUN_CODE_MEMORYLIMIT;

        private readonly ObservableCollection<Language> Languages = new();
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Read Run Code Time Limit and Memory Limit from the Setting
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var CurrentTimeLimit = localSettings.Values[TIMELIMIT_KEY];
            if (CurrentTimeLimit != null)
            {
                _timeLimit = (int)CurrentTimeLimit;
            }
            else
            {
                localSettings.Values[TIMELIMIT_KEY] = DEFAULT_RUN_CODE_TIMELIMIT;
            }
            var CurrentMemoryLimit = localSettings.Values[MEMORYLIMIT_KEY];
            if (CurrentMemoryLimit != null)
            {
                _memoryLimit = (int)CurrentMemoryLimit;
            }
            else
            {
                localSettings.Values[MEMORYLIMIT_KEY] = DEFAULT_RUN_CODE_MEMORYLIMIT;
            }

            var progress = new Progress<int>(percent => { RunCodeProgressBar.Value = percent; });

            RunCodeButton.IsEnabled = false;

            RunCodeResult result = await Judger.RunCode(CodeEditor.Code, Input, Languages[LanguageComboBox.SelectedIndex], _timeLimit, _memoryLimit, progress);

            RunCodeButton.IsEnabled = true;

            StatusTextBlock.Text = $"{result.Result} Time: {result.CPUTime} ms Memory: {result.MemoryUsage / 1024 / 1024} MB";
            OutputBox.Text = result.StandardOutput + result.StandardError;

        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Language language = LanguageComboBox.SelectedItem as Language;
            CodeEditor.Lang = language.Name;
        }
    }
}
