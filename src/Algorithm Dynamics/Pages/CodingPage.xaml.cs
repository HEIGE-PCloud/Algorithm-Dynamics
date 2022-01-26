using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CodingPage : Page
    {
        public string Code { get; set; }
        public CodingPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<Submission> Submissions = new() {
            new Submission(DateTime.Now, "Accepted", "8 ms", "9 MB", "cpp")
        };
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
    }
    public class Submission
    {
        public Submission(DateTime timeSubmitted, string status, string runtime, string memory, string language)
        {
            TimeSubmitted = timeSubmitted;
            Status = status;
            Runtime = runtime;
            Memory = memory;
            Language = language;
        }

        public DateTime TimeSubmitted { get; set; }
        public string Status { get; set; }
        public string Runtime { get; set; }
        public string Memory { get; set; }
        public string Language { get; set; }
    }
}
