using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                    OnPropertyChanged(nameof(_currentProblemIndex));
                }
            }
        }
        private List<Problem> _currentProblemList;
        private int _currentProblemIndex
        {
            get
            {
                return _currentProblemList.IndexOf(_currentProblem);
            }
        }

        public string Code { get; set; }
        public CodingPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<Submission> Submissions = new() 
        {
            new Submission(DateTime.Now, "Accepted", "8 ms", "9 MB", "cpp", "hello world"),
        };

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
            CodeEditor.Lang = LanguageComboBox.SelectedItem as string;
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

        private void SubmissionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Submission submission = SubmissionsDataGrid.SelectedItem as Submission;
            CodeEditor.Code = submission.Code;
            LanguageComboBox.SelectedValue = submission.Language;
        }

        private void ProblemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var problem = ProblemListView.SelectedItem as Problem;
            CurrentProblem = problem;
        }

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
    }
    public class Submission
    {
        public Submission(DateTime timeSubmitted, string status, string runtime, string memory, string language, string code)
        {
            TimeSubmitted = timeSubmitted;
            Status = status;
            Runtime = runtime;
            Memory = memory;
            Language = language;
            Code = code;
        }

        public DateTime TimeSubmitted { get; set; }
        public string Status { get; set; }
        public string Runtime { get; set; }
        public string Memory { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
    }
}
