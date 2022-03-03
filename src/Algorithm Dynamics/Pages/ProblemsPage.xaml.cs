using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System;
using Algorithm_Dynamics.Helpers;
using Windows.Storage;
using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class ProblemsPage : Page
    {
        public ProblemsPage()
        {
            InitializeComponent();
            List<Problem> problems = DataAccess.GetAllProblems();
            //if (problems.Count == 0)
            //{
            //    Problem problem1 = Problem.Create(Guid.NewGuid(), "Problem 1", "Description 1", 1000, 64 * 1024 * 1024, ProblemStatus.Todo, Difficulty.Easy);
            //    Problem problem2 = Problem.Create(Guid.NewGuid(), "Problem 2", "Description 2", 2000, 6 * 1024 * 1024, ProblemStatus.Attempted, Difficulty.Hard);
            //    Problem problem3 = Problem.Create(Guid.NewGuid(), "Problem 3", "Description 3", 3000, 64 * 1024 * 1024, ProblemStatus.Solved, Difficulty.Medium);

            //}
            //problems = DataAccess.GetAllProblems();

            foreach (var problem in problems)
            {
                Problems.Add(problem);
            }
            foreach (var tag in DataAccess.GetAllTags())
            {
                Tags.Add(tag.Name);
            }
        }
        private readonly ObservableCollection<string> Difficulties = new() { "Easy", "Medium", "Hard" };
        private readonly ObservableCollection<string> Statuses = new() { "Todo", "Attempted", "Done" };
        public ObservableCollection<string> Lists = new() { "List 1", "List 2", "List 3" };
        public ObservableCollection<string> Tags = new();
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
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.EditProblemList));
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
            App.NavigateTo(typeof(CreateNewProblemPage), Tuple.Create(CreateNewProblemPage.Mode.Edit, (Problem)ProblemsListView.SelectedItem));
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
            //Problems.Clear();
            //Problems.Add(new Problem(keywords, difficulty, status, tag));
        }

        /// <summary>
        /// Navigate to the <see cref="CodingPage"/>, pass the current Problem and ProblemList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartProblem(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var problem = (Problem)button.DataContext;
            App.NavigateTo(typeof(CodingPage), Tuple.Create(problem, Problems.ToList()));
        }

        /// <summary>
        /// Navigate to the <see cref="CreateNewProblemPage"/> to create a new <see cref="Problem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProblem(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemPage));
        }

        private async void Import(object sender, RoutedEventArgs e)
        {
            StorageFile file = await FileHelper.FileOpenPicker("*");
        }

        private void CreateNewProblemList(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage));
        }

        private void Search(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void Search(object sender, SelectionChangedEventArgs e)
        {
            Problems.Clear();
            var problems = DataAccess.GetAllProblems();
            //if (DifficultyComboBox.SelectedIndex == -1 && TagComboBox.SelectedIndex == -1 && ListComboBox.SelectedIndex == -1 && StatusComboBox.SelectedIndex == -1)
            //{

            //    return;
            //}

            if (DifficultyComboBox.SelectedIndex != -1)
            {
                var difficulty = (Difficulty)DifficultyComboBox.SelectedIndex;
                foreach (var problem in problems)
                {
                    if (problem.Difficulty != difficulty)
                        problems.Remove(problem);
                }
            }

            foreach (var problem in problems)
            {
                Problems.Add(problem);
            }

        }
    }
}
