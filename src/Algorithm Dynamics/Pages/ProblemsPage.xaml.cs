using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class ProblemsPage : Page
    {
        public ProblemsPage()
        {
            InitializeComponent();
            RefreshDatabase();
        }
        public ObservableCollection<ProblemList> ProblemLists = new();
        public ObservableCollection<Tag> Tags = new();
        public ObservableCollection<Problem> Problems = new();

        public void RefreshDatabase()
        {
            ProblemLists.Clear();
            Tags.Clear();
            Problems.Clear();

            Problem.All.ForEach(problem => Problems.Add(problem));
            Core.Models.Tag.All.ForEach(tag => Tags.Add(tag));
            ProblemList.All.ForEach(problemList => ProblemLists.Add(problemList));
        }

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
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.EditProblemList, (ProblemList)ListComboBox.SelectedItem, new List<Problem>()));
        }

        /// <summary>
        /// Show a content dialog to confirm the deletion of a ProblemList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void DeleteProblemList(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                Title = "Delete Problem List",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                Content = $"Are you sure that you want to permanently delete {ListComboBox.SelectedItem}?",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var problemList = (ProblemList)ListComboBox.SelectedItem;
                problemList.Delete();
                RefreshDatabase();
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
            ContentDialog dialog = new()
            {
                Title = "Delete Problem",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                Content = $"Are you sure that you want to permanently delete {(ProblemsListView.SelectedItem as Problem).Name}?",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var problem = (Problem)ProblemsListView.SelectedItem;
                problem.Delete();
                RefreshDatabase();
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
            List<Problem> problemList = new();
            foreach (Problem item in ProblemsListView.SelectedItems.ToArray())
            {
                problemList.Add(item);
            }
            App.NavigateTo(typeof(CreateNewProblemListPage), Tuple.Create(CreateNewProblemListPage.Mode.CreateProblemList, (ProblemList)ListComboBox.SelectedItem, problemList));
        }

        /// <summary>
        /// Show a content dialog to confirm the deletion of Problems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void DeleteProblems(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new()
            {
                Title = "Delete Problem",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel",
                Content = $"Are you sure that you want to permanently delete these {ProblemsListView.SelectedItems.Count} Problems?",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = Content.XamlRoot
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var problems = ProblemsListView.SelectedItems;
                foreach (Problem problem in problems) problem.Delete();
                RefreshDatabase();
            }
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
            IReadOnlyList<StorageFile> files = await FileHelper.FileOpenPicker(".json");
            if (files.Count > 0)
            {
                foreach (StorageFile file in files)
                {
                    try
                    {
                        string data = await FileIO.ReadTextAsync(file);
                        string dataType = DataSerialization.GetDataType(data);
                        if (dataType == "Problem")
                        {
                            DataSerialization.DeserializeProblem(data);
                        }
                        else if (dataType == "ProblemList")
                        {
                            DataSerialization.DeserializeProblemList(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        ContentDialog dialog = new()
                        {
                            Title = $"An error was encountered while importing {file.DisplayName}",
                            PrimaryButtonText = "Ok",
                            Content = $"The following error is encountered:\n\n{ex.Message}",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = Content.XamlRoot
                        };
                        await dialog.ShowAsync();
                    }
                }
                RefreshDatabase();
            }
        }

        private void CreateNewProblemList(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage));
        }

        private void Search(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Search();
        }

        private void Search(object sender, SelectionChangedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            Problems.Clear();
            NoResultTextBlock.Visibility = Visibility.Collapsed;
            var problems = Problem.All;

            if (DifficultyComboBox.SelectedIndex != -1)
            {
                var difficulty = (Difficulty)DifficultyComboBox.SelectedIndex;
                problems.RemoveAll(p => p.Difficulty != difficulty);
            }

            if (StatusComboBox.SelectedIndex != -1)
            {
                var status = (ProblemStatus)StatusComboBox.SelectedIndex;
                problems.RemoveAll(p => p.Status != status);
            }

            if (TagComboBox.SelectedIndex != -1)
            {
                var tag = (Tag)TagComboBox.SelectedItem;
                problems.RemoveAll(p => p.Tags.Contains(tag) == false);
            }

            if (ListComboBox.SelectedIndex != -1)
            {
                var list = (ProblemList)ListComboBox.SelectedItem;
                problems.RemoveAll(p => list.Problems.Contains(p) == false);
            }

            if (string.IsNullOrEmpty(ProblemsSearchBox.Text) == false)
            {
                var name = ProblemsSearchBox.Text;
                problems.RemoveAll(p => p.Name != name);
            }

            if (problems.Count == 0)
                NoResultTextBlock.Visibility = Visibility.Visible;

            foreach (var problem in problems)
            {
                Problems.Add(problem);
            }
        }

        private void ProblemsSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(ProblemsSearchBox.Text))
            {
                Search();
            }
            else
            {
                string keyword = ProblemsSearchBox.Text.Trim();
                var resultList = new List<Problem>();
                var sourceList = Problem.All;
                var splitKeyword = keyword.ToLower().Split(' ');
                for (int i = 0; i < sourceList.Count; i++)
                {
                    for (int j = 0; j < splitKeyword.Length; j++)
                    {
                        var sourceKey = sourceList[i].Name.ToLower();
                        if (sourceKey.Contains(splitKeyword[j]))
                        {
                            resultList.Add(sourceList[i]);
                            break;
                        }
                    }
                }
                sender.ItemsSource = resultList.Select(p => p.Name).ToList();
            }
        }

        private async void ExportProblem(object sender, RoutedEventArgs e)
        {
            Problem problem = ProblemsListView.SelectedItem as Problem;
            string fileName = $"{problem.Name} Export";
            string serializedProblem = DataSerialization.SerializeProblem(problem);
            await FileHelper.FileSavePicker("Algorithm Dynamics Export File", new() { ".json" }, fileName, serializedProblem);
        }

        private async void ExportProblemList(object sender, RoutedEventArgs e)
        {
            ProblemList problemList = ListComboBox.SelectedItem as ProblemList;
            string fileName = $"{problemList.Name} Export";
            string serializedProblem = DataSerialization.SerializeProblemList(problemList);
            await FileHelper.FileSavePicker("Algorithm Dynamics Export File", new() { ".json" }, fileName, serializedProblem);
        }
    }
}
