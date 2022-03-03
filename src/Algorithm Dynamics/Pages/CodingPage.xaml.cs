using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CodingPage : Page
    {
        private Problem _currentProblem;
        private List<Problem> _currentProblemList;
        public string ProblemDescription
        {
            get
            {
                // TODO: compose time limit and example test cases
                if (_currentProblem != null)
                    return _currentProblem.Description;
                return "";
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
