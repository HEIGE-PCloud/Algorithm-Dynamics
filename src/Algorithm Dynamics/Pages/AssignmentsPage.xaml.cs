using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentsPage : Page
    {
        public AssignmentsPage()
        {
            InitializeComponent();
            AssignmentsNavView.SelectedItem = AssignmentsNavView.MenuItems[0];

        }
        public ObservableCollection<Assignment> Assignments = new();

        /// <summary>
        /// Set the <see cref="Assignments"/> based on different SelectedItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AssignmentsNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (AssignmentsNavView.SelectedItem == AssignmentsNavView.MenuItems[0])
            {
                Assignments.Clear();
                Assignments.Add(new Assignment("Assignment 1", "Assigned Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 2", "Assigned Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 3", "Assigned Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 4", "Assigned Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 5", "Assigned Assignment", DateTime.Now));
            }
            else if (AssignmentsNavView.SelectedItem == AssignmentsNavView.MenuItems[1])
            {
                Assignments.Clear();
                Assignments.Add(new Assignment("Assignment 6", "Completed Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 7", "Completed Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 8", "Completed Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 9", "Completed Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 10", "Completed Assignment", DateTime.Now));
            }
            else
            {
                Assignments.Clear();
                Assignments.Add(new Assignment("Assignment 11", "Created Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 12", "Created Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 13", "Created Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 14", "Created Assignment", DateTime.Now));
                Assignments.Add(new Assignment("Assignment 15", "Created Assignment", DateTime.Now));
            }
        }

        /// <summary>
        /// Navigate the the <see cref="AssignmentDetailsPage"/> when an assignment is clicked in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var assignment = e.ClickedItem as Assignment;
            if (assignment.Description == "Created Assignment")
            {
                App.NavigateTo(typeof(AssignmentDetailsPage), Tuple.Create(AssignmentDetailsPage.Mode.Teacher));
            }
            else
            {
                App.NavigateTo(typeof(AssignmentDetailsPage), Tuple.Create(AssignmentDetailsPage.Mode.Student));
            }
        }

        /// <summary>
        /// Navigate to the <see cref="CreateNewProblemListPage"/> with <see cref="CreateNewProblemListPage.Mode.CreateAssignment"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateAssignment(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.CreateAssignment));
        }

        /// <summary>
        /// Display a <see cref="FileOpenPicker"/> and load the assignment from the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ImportAssignment(object sender, RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            IntPtr hwnd = WindowNative.GetWindowHandle(App.m_window);

            // Associate the HWND with the file picker
            InitializeWithWindow.Initialize(filePicker, hwnd);

            // Use file picker
            filePicker.FileTypeFilter.Add("*");
            StorageFile file = await filePicker.PickSingleFileAsync();
            // TODO: Import file into the Database.
        }
    }

    public class Assignment
    {
        public Assignment(string name, string description, DateTime dueDate)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
