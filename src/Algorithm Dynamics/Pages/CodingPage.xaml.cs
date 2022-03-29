using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CodingPage : Page, INotifyPropertyChanged
    {
        private Problem _currentProblem;
        public Problem CurrentProblem
        {
            get => _currentProblem;
            set
            {
                if (_currentProblem != value)
                {
                    _currentProblem = value;
                    OnPropertyChanged(nameof(CurrentProblem));
                    OnPropertyChanged(nameof(_currentProblem));
                    OnPropertyChanged(nameof(CurrentProblemIndex));
                    OnPropertyChanged(nameof(ProblemMarkdown));
                    OnPropertyChanged(nameof(Submissions));
                    OnPropertyChanged(nameof(ReverseSubmissions));
                }
            }
        }
        private List<Problem> _currentProblemList;
        private readonly ObservableCollection<Language> Languages = new();

        public int CurrentProblemIndex { get => _currentProblemList.IndexOf(_currentProblem); }

        public string Code { get; set; }

        /// <summary>
        /// The renderec markdown text of the current problem
        /// </summary>
        public string ProblemMarkdown
        {
            get
            {
                if (_currentProblem != null)
                {
                    const int MB = 1024 * 1024;
                    
                    // Set title
                    string title = $"# {_currentProblem.Name}\n\n";
                    
                    // Set time limit
                    string timeLimit = $"\n\n## Time Limit\n\n{_currentProblem.TimeLimit} ms";
                    
                    // Set memory limit
                    string memoryLimit = $"\n## Memory Limit\n\n{_currentProblem.MemoryLimit / MB} MB";
                    
                    // Set example test case
                    string example = "\n## Example";
                    int testCaseCnt = 1;
                    _currentProblem.TestCases.Where(testCase => testCase.IsExample == true).ToList().ForEach(testCase =>
                    {
                        example += $"\n### Example Input {testCaseCnt}\n";
                        example += "```\n" + testCase.Input.Replace("\n", "\n\n") + "\n```\n";
                        example += $"\n### Example Output {testCaseCnt}\n";
                        example += "```\n" + testCase.Output.Replace("\n", "\n\n") + "\n```\n";
                        testCaseCnt++;
                    });

                    return title + _currentProblem.Description + timeLimit + memoryLimit + example;
                }
                return "";
            }
        }

        public CodingPage()
        {
            InitializeComponent();
            Core.Models.Language.All.ForEach(lang => Languages.Add(lang));
        }
        public ObservableCollection<SubmissionResult> Submissions { get => new(SubmissionResult.All.Where(result => result.Submission.Problem.Id == _currentProblem.Id)); }
        public ObservableCollection<SubmissionResult> ReverseSubmissions { get => new(Submissions.Reverse()); }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke a new <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Use <see cref="nameof"/> to get the name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var parameter = (Tuple<Problem, List<Problem>>)e.Parameter;
                _currentProblem = parameter.Item1;
                _currentProblemList = parameter.Item2;
            }
            base.OnNavigatedTo(e);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Language language = LanguageComboBox.SelectedItem as Language;
            CodeEditor.Lang = language.Name;
        }
        /// <summary>
        /// Toggle the fullscreen mode for the app
        /// Toggle the <see cref="AppWindowPresenterKind"/> and <see cref="FullScreenIcon.Glyph"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            AppWindow window = MainWindow.AppWindow;
            if (window.Presenter.Kind == AppWindowPresenterKind.Overlapped)
            {
                window.SetPresenter(AppWindowPresenterKind.FullScreen);
                FullScreenIcon.Glyph = "\xE73F";
            }
            else
            {
                window.SetPresenter(AppWindowPresenterKind.Overlapped);
                FullScreenIcon.Glyph = "\xE740";
            }
        }
        
        /// <summary>
        /// Load the submission code and language config to the code editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmissionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubmissionsDataGrid.SelectedItem is SubmissionResult result)
            {
                CodeEditor.Code = result.Submission.Code;
                LanguageComboBox.SelectedItem = result.Submission.Language;
            }
        }

        /// <summary>
        /// Load the problem when navigate within coding page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProblemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var problem = ProblemListView.SelectedItem as Problem;
            CurrentProblem = problem;
        }

        /// <summary>
        /// Navigate to the previous problem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousProblem(object sender, RoutedEventArgs e)
        {
            var problem = ProblemListView.SelectedItem as Problem;
            int index = _currentProblemList.IndexOf(problem);
            if (index == 0)
            {
                CurrentProblem = _currentProblemList[_currentProblemList.Count - 1];
            }
            else
            {
                CurrentProblem = _currentProblemList[index - 1];
            }
        }

        /// <summary>
        /// Navigate to the next problem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextProblem(object sender, RoutedEventArgs e)
        {
            var problem = ProblemListView.SelectedItem as Problem;
            int index = _currentProblemList.IndexOf(problem);
            if (index == _currentProblemList.Count - 1)
            {
                CurrentProblem = _currentProblemList[0];
            }
            else
            {
                CurrentProblem = _currentProblemList[index + 1];
            }
        }

        /// <summary>
        /// Run the user's code with the given input 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the progress bar and button
            var progress = new Progress<int>(percent => { RunCodeProgressBar.Value = percent; });
            RunCodeButton.IsEnabled = false;
            SubmitCodeButton.IsEnabled = false;

            // RunCode
            RunCodeResult result = await Judger.RunCode(CodeEditor.Code, InputTextBox.Text, Languages[LanguageComboBox.SelectedIndex], _currentProblem.TimeLimit, CurrentProblem.MemoryLimit, progress);

            // Restore button
            RunCodeButton.IsEnabled = true;
            SubmitCodeButton.IsEnabled = true;

            // Display the result
            StatusTextBlock.Text = $"{result.Result} Time: {result.CPUTime} ms Memory: {result.MemoryUsage / 1024 / 1024} MB";
            OutputTextBox.Text = result.StandardOutput;
            ErrorTextBox.Text = result.StandardError;

            // Navigate to the output or error panel
            if (string.IsNullOrEmpty(result.StandardError))
            {
                IOPivot.SelectedIndex = 1;
            }
            else
            {
                IOPivot.SelectedIndex = 2;
            }
        }

        /// <summary>
        /// Submit the user's code for formal judging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SubmitCodeButton_Click(object sender, RoutedEventArgs e)
        {
            // Create submission
            Submission submission = Submission.Create(CodeEditor.Code, (Language)LanguageComboBox.SelectedItem, App.CurrentUser, _currentProblem);
            
            // Prepare progerss bar and button
            var progress = new Progress<int>(percent => { RunCodeProgressBar.Value = percent; });
            RunCodeButton.IsEnabled = false;
            SubmitCodeButton.IsEnabled = false;

            // Judge problem
            SubmissionResult result = await Judger.JudgeProblem(submission, progress);

            // Restore button
            RunCodeButton.IsEnabled = true;
            SubmitCodeButton.IsEnabled = true;

            // Display the result
            StatusTextBlock.Text = $"{result.Result} Time: {result.CPUTime} ms Memory: {result.MemoryUsage / 1024 / 1024} MB";
            ErrorTextBox.Text = result.StandardError;
            
            // Navigate to stderr
            if (!string.IsNullOrWhiteSpace(result.StandardError))
            {
                IOPivot.SelectedIndex = 2;
            }
            OnPropertyChanged(nameof(Submissions));
            OnPropertyChanged(nameof(ReverseSubmissions));
        }
    }
}
