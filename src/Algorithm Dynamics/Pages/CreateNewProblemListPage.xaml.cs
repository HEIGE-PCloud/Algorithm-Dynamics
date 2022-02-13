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
    public sealed partial class CreateNewProblemListPage : Page
    {
        public CreateNewProblemListPage()
        {
            InitializeComponent();
        }
        public enum Mode
        {
            CreateProblemList,
            EditProblemList,
            CreateAssignment,
            EditAssignment
        }
        private Mode PageMode = Mode.CreateProblemList;
        private string _title
        {
            get
            {
                if (PageMode == Mode.CreateProblemList)
                    return "Create Problem List";
                else if (PageMode == Mode.EditProblemList)
                    return "Edit Problem List";
                else if (PageMode == Mode.CreateAssignment)
                    return "Create Assignment";
                else
                    return "Edit Assignment";
            }
        }

        /// <summary>
        /// Handle the Navigation Arguments
        /// Set the <see cref="PageMode"/> if the Parameter is not <see cref="null"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                PageMode = ((Tuple<Mode, int>)e.Parameter).Item1;
            }
            base.OnNavigatedTo(e);
        }


        private ObservableCollection<Problem> Problems = new() { new Problem("Problem 1", "Hard", "ToDo", "Data structure") };

        private void AddProblemBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                suitableItems.Add(sender.Text);
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

        }

        private void AddProblemBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Problems.Add(new Problem(sender.Text, "Easy", "ToDo", "Data structure"));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TestTextBlock.Text = "The Problem List is saved.";
        }

        private void DeleteSingleTestCase(object sender, RoutedEventArgs e)
        {
            Problem selectedItem = ((FrameworkElement)sender).DataContext as Problem;
            Problems.Remove(selectedItem);
        }

        /// <summary>
        /// Finish create or edit the ProblemList or Assignment.
        /// Go back to <see cref="ProblemsPage"/> or <see cref="AssignmentsPage"/> based on <see cref="PageMode"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Finish(object sender, RoutedEventArgs e)
        {
            SaveFlyout.Hide();
            if (PageMode == Mode.CreateProblemList || PageMode == Mode.EditProblemList)
                App.NavigateTo(typeof(ProblemsPage));
            else
                App.NavigateTo(typeof(AssignmentsPage));
        }
    }
}
