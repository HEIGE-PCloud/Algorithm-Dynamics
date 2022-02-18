﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using WinRT.Interop;

namespace Algorithm_Dynamics.Pages
{
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
        private Mode _pageMode = Mode.CreateProblemList;
        private bool _isAssignmentMode { get => _pageMode == Mode.CreateAssignment || _pageMode == Mode.EditAssignment; }
        private string _title
        {
            get
            {
                if (_pageMode == Mode.CreateProblemList)
                    return "Create Problem List";
                else if (_pageMode == Mode.EditProblemList)
                    return "Edit Problem List";
                else if (_pageMode == Mode.CreateAssignment)
                    return "Create Assignment";
                else
                    return "Edit Assignment";
            }
        }

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

        private ObservableCollection<Problem> Problems = new() { new Problem("Problem 1", "Hard", "ToDo", "Data structure") };

        /// <summary>
        /// When the text is changed in the search box, search in the database and create suggestion items. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddProblemBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                suitableItems.Add(sender.Text);
                // TODO: search the actual problem from the database
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }

        }

        /// <summary>
        /// When a suggestion is chosen, add it in to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddProblemBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Problems.Add(new Problem(sender.Text, "Easy", "ToDo", "Data structure"));
        }

        /// <summary>
        /// When the save button is clicked, save the problem list and change the description test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: actually save the problem to the database.
            TestTextBlock.Text = "The Problem List is saved.";
        }

        /// <summary>
        /// Delete the selected problem from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSingleProblem(object sender, RoutedEventArgs e)
        {
            Problem selectedItem = ((FrameworkElement)sender).DataContext as Problem;
            Problems.Remove(selectedItem);
        }

        /// <summary>
        /// Finish create or edit the ProblemList or Assignment.
        /// Go back to <see cref="ProblemsPage"/> or <see cref="AssignmentsPage"/> based on <see cref="_pageMode"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Finish(object sender, RoutedEventArgs e)
        {
            SaveFlyout.Hide();
            if (_pageMode == Mode.CreateProblemList || _pageMode == Mode.EditProblemList)
                App.NavigateTo(typeof(ProblemsPage));
            else
                App.NavigateTo(typeof(AssignmentsPage));
        }

        /// <summary>
        /// Display the <see cref="FileSavePicker"/> to get the file path.
        /// Save the file and finish.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportAndFinish(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker();
            IntPtr hwnd = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(savePicker, hwnd);
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("JSON file", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "Assignment Name";
            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                await FileIO.WriteTextAsync(file, "TODO: export the Problem List/Assignment");
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    SaveFlyout.Hide();
                    if (_pageMode == Mode.CreateProblemList || _pageMode == Mode.EditProblemList)
                        App.NavigateTo(typeof(ProblemsPage));
                    else
                        App.NavigateTo(typeof(AssignmentsPage));
                }
                else
                {
                    // TODO: handle saved failed
                }
            }
            else
            {
                // TODO: handle cancelled
            }
        }
    }
}
