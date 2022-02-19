using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentDetailsPage : Page
    {
        public AssignmentDetailsPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<Problem> Problems = new()
        { 
            new Problem("Problem 1", "Easy", "ToDo", "Tag"), 
            new Problem("Problem 2", "Easy", "ToDo", "Tag"), 
            new Problem("Problem 3", "Easy", "ToDo", "Tag"), 
            new Problem("Problem 4", "Easy", "ToDo", "Tag"), 
            new Problem("Problem 5", "Easy", "ToDo", "Tag"), 
        };
        public ObservableCollection<Submission> Submissions = new() 
        { 
            new Submission(DateTime.Now, "Submission 1", "RunTime", "Memory", "Language", "Code"),
            new Submission(DateTime.Now, "Submission 2", "RunTime", "Memory", "Language", "Code"),
            new Submission(DateTime.Now, "Submission 3", "RunTime", "Memory", "Language", "Code"),
            new Submission(DateTime.Now, "Submission 4", "RunTime", "Memory", "Language", "Code"),
            new Submission(DateTime.Now, "Submission 5", "RunTime", "Memory", "Language", "Code") 
        };
        public enum Mode
        {
            Student,
            Teacher
        }
        private Mode _pageMode = Mode.Student;
        private bool _isStudentMode { get => _pageMode == Mode.Student; }
        private bool _isTeacherMode { get => _pageMode == Mode.Teacher; }

        /// <summary>
        /// Handle the Navigation Arguments
        /// Set the <see cref="_pageMode"/> if the Parameter is not <see cref="null"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                _pageMode = ((Tuple<Mode>)e.Parameter).Item1;
            }
            base.OnNavigatedTo(e);
        }

        private void ProblemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.NavigateTo(typeof(CodingPage));
        }

        private void EditAssignment(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.EditAssignment));
        }
    }
}
