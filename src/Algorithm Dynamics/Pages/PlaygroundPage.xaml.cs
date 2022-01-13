using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Algorithm_Dynamics.Core.Models;
using System;
using System.Collections.ObjectModel;

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

        private ObservableCollection<Language> languages = new() { LanguageConfig.C, LanguageConfig.Cpp, LanguageConfig.Python, LanguageConfig.JavaScript, LanguageConfig.Rust, LanguageConfig.Go, LanguageConfig.Java};
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>(percent => { RunCodeProgressBar.Value = percent; });
            RunCodeProgressBar.IsIndeterminate = true;
            RunCodeProgressBar.ShowError = false;
            RunCodeButton.IsEnabled = false;
            RunCodeResult result = await Judger.RunCode(Code, Input, languages[LanguageComboBox.SelectedIndex], 1000, 1000000000, progress);
            RunCodeButton.IsEnabled = true;
            RunCodeProgressBar.IsIndeterminate = false;
            StatusTextBlock.Text = result.ResultCode.ToString();
            OutputBox.Text = result.StandardOutput + result.StandardError;
            if (result.ResultCode != ResultCode.SUCCESS)
                RunCodeProgressBar.ShowError = true;
            else
                RunCodeProgressBar.ShowError = false;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CodeEditor.Lang = LanguageComboBox.SelectedItem.ToString();
        }
    }
}
