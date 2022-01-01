using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class ProblemsPage : Page
    {
        public ProblemsPage()
        {
            InitializeComponent();
            Problems.Add(new Problem("Problem 1", "Easy", "Todo", "Tag"));
            Problems.Add(new Problem("Problem 2", "Easy", "Attempted", "Tag"));
            Problems.Add(new Problem("Problem 3", "Easy", "Done", "Tag"));
            Problems.Add(new Problem("Problem 4", "Medium", "Todo", "Tag"));
            Problems.Add(new Problem("Problem 5", "Medium", "Attempted", "Tag"));
            Problems.Add(new Problem("Problem 6", "Medium", "Done", "Tag"));
            Problems.Add(new Problem("Problem 7", "Hard", "Todo", "Tag"));
            Problems.Add(new Problem("Problem 8", "Hard", "Attempted", "Tag"));
            Problems.Add(new Problem("Problem 9", "Hard", "Done", "Tag"));
            Problems.Add(new Problem("Problem 10", "Hard", "Todo", "Tag"));
        }
        private readonly ObservableCollection<string> Difficulties = new() { "Easy", "Medium", "Hard" };
        private readonly ObservableCollection<string> Statuses = new() { "Todo", "Attempted", "Done" };
        public ObservableCollection<string> Lists = new() { "List 1", "List 2", "List 3" };
        public ObservableCollection<string> Tags = new() { "Tag 1", "Tag 2", "Tag 3"};
        public ObservableCollection<Problem> Problems = new();
        /// <summary>
        /// Display the <see cref="ListMenuFlyout"/> when the <see cref="ListComboBox"/> is right tapped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListComboBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedIndex != -1)
            {
                ListMenuFlyout.ShowAt(comboBox, e.GetPosition(comboBox));
            }
        }

        /// <summary>
        /// Clear the <see cref="ComboBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearComboBox(object sender, RightTappedRoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Clear the <see cref="ListComboBox"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearComboBox(object sender, RoutedEventArgs e)
        {
            ListComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Navigate to the edit ProblemList page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EditProblemList(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to edit page
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show a content dialog to confirm the deletion of a ProblemList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void DeleteProblemList(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Delete Problem List";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Are you sure that you want to permanently delete {ListComboBox.SelectedItem}?";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.XamlRoot = this.Content.XamlRoot;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // TODO: Delete the selected problem list
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Show flyout when the user right tapped the ProblemListView
        /// When one problem is selected, show the SingleSelectedMenuFlyout
        /// When mutiple problems are selected, show the MultipleSelectedMenuFlyout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProblemsListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 1)
            {
                SingleSelectedMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            }
            else if (listView.SelectedItems.Count > 1)
            {
                MultipleSelectedMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            }
        }

        /// <summary>
        /// Edit the selected Problem
        /// Navigate to the EditProblemPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditProblem(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to the EditProblemPage
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show a content dialog to confirm the deletion of a Problem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void DeleteProblem(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Delete Problem";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Are you sure that you want to permanently delete {(ProblemsListView.SelectedItem as Problem).Name}?";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.XamlRoot = this.Content.XamlRoot;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // TODO: Delete the selected problem
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create a new ProblemList from multiple selected items
        /// Navigate to the CreateProblemListPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CreateProblemList(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to the CreateProblemListPage
            throw new NotImplementedException();
        }

        /// <summary>
        /// Show a content dialog to confirm the deletion of Problems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void DeleteProblems(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Delete Problem";
            dialog.PrimaryButtonText = "Delete";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = $"Are you sure that you want to permanently delete these {ProblemsListView.SelectedItems.Count} Problems?";
            dialog.DefaultButton = ContentDialogButton.Close;
            dialog.XamlRoot = this.Content.XamlRoot;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // TODO: Delete the selected problems
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Search the problem lists that under conditions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Search(object sender, object args)
        {
            var keywords = ProblemsSearchBox.Text;
            var difficulty = DifficultyComboBox.SelectedValue?.ToString();
            var status = StatusComboBox.SelectedValue?.ToString();
            var tag = TagComboBox.SelectedValue?.ToString();
            var list = ListComboBox.SelectedValue?.ToString();
            // TODO: Query(keywords, difficulty, status, tag, list);
            Problems.Clear();
            Problems.Add(new Problem(keywords, difficulty, status, tag));
        }
    }
    public class Problem
    {
        public Problem(string name, string difficulty, string status, string tags)
        {
            Name = name;
            Difficulty = difficulty;
            Status = status;
            Tags = tags;
        }

        public string Name { get; set; }
        public string Difficulty { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
    }
}
