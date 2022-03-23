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
        private ObservableCollection<ProblemList> ProblemLists = new();
        private ObservableCollection<Tag> Tags = new();
        private ObservableCollection<Problem> Problems = new();

        /// <summary>
        /// Reload all data from the database
        /// </summary>
        private void RefreshDatabase()
        {
            // Clear the existing lists
            ProblemLists.Clear();
            Tags.Clear();
            Problems.Clear();

            // Load all data from the database
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
                        // Read data
                        string data = await FileIO.ReadTextAsync(file);

                        // Get data type
                        string dataType = DataSerialization.GetDataType(data);

                        // Deserialize the data and save to the database
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
                        // Show an dialog with the error message
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

        /// <summary>
        /// Navigate to the <see cref="CreateNewProblemListPage"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewProblemList(object sender, RoutedEventArgs e)
        {
            App.NavigateTo(typeof(CreateNewProblemListPage));
        }


        private void Search(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
            => Query();

        private void Search(object sender, SelectionChangedEventArgs e)
            => Query();

        private void Query()
        {
            // Clear existing results
            Problems.Clear();
            NoResultTextBlock.Visibility = Visibility.Collapsed;
            
            // Get all problems
            var problems = Problem.All;

            // Query difficulty
            if (DifficultyComboBox.SelectedIndex != -1)
            {
                var difficulty = (Difficulty)DifficultyComboBox.SelectedIndex;
                problems.RemoveAll(p => p.Difficulty != difficulty);
            }

            // Query status
            if (StatusComboBox.SelectedIndex != -1)
            {
                var status = (ProblemStatus)StatusComboBox.SelectedIndex;
                problems.RemoveAll(p => p.Status != status);
            }

            // Query tag
            if (TagComboBox.SelectedIndex != -1)
            {
                var tag = (Tag)TagComboBox.SelectedItem;
                problems.RemoveAll(p => p.Tags.Contains(tag) == false);
            }

            // Query list
            if (ListComboBox.SelectedIndex != -1)
            {
                var list = (ProblemList)ListComboBox.SelectedItem;
                problems.RemoveAll(p => list.Problems.Contains(p) == false);
            }

            // Query name
            if (string.IsNullOrEmpty(ProblemsSearchBox.Text) == false)
            {
                var name = ProblemsSearchBox.Text;
                problems.RemoveAll(p => p.Name != name);
            }

            // Handle no result
            if (problems.Count == 0)
                NoResultTextBlock.Visibility = Visibility.Visible;

            // Return results
            foreach (var problem in problems)
            {
                Problems.Add(problem);
            }
        }

        /// <summary>
        /// Give fuzzy search suggestions when the user is inputing search query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ProblemsSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Query if empty
            if (string.IsNullOrEmpty(ProblemsSearchBox.Text))
            {
                Query();
            }
            else
            {
                // Give fuzzy search suggestions
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
                // Return result
                sender.ItemsSource = resultList.Select(p => p.Name).ToList();
            }
        }

        /// <summary>
        /// Export problem to a JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportProblem(object sender, RoutedEventArgs e)
        {
            // Get selected problem
            Problem problem = ProblemsListView.SelectedItem as Problem;
            
            // Set file name
            string fileName = $"{problem.Name} Export";
            
            // Serialize problem
            string serializedProblem = DataSerialization.SerializeProblem(problem);
            
            // Save problem
            await FileHelper.FileSavePicker("Algorithm Dynamics Export File", new() { ".json" }, fileName, serializedProblem);
        }

        /// <summary>
        /// Export problem list to a JSON file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportProblemList(object sender, RoutedEventArgs e)
        {
            // Get selected problem list
            ProblemList problemList = ListComboBox.SelectedItem as ProblemList;
            
            // Set file name
            string fileName = $"{problemList.Name} Export";
            
            // Serialize problem
            string serializedProblem = DataSerialization.SerializeProblemList(problemList);
            
            // Save problem list
            await FileHelper.FileSavePicker("Algorithm Dynamics Export File", new() { ".json" }, fileName, serializedProblem);
        }
    }
}
