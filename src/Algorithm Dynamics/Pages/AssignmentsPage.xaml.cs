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
    public sealed partial class AssignmentsPage : Page
    {
        public AssignmentsPage()
        {
            InitializeComponent();
            AssignmentsNavView.SelectedItem = AssignmentsNavView.MenuItems[0];

        }
        public ObservableCollection<Assignment> Assignments = new();

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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.NavigateTo(typeof(AssignmentDetailsPage));
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
