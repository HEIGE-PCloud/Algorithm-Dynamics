using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private ProblemList _problemList;
        private string _name = "";
        private string _description = "";

        /// <summary>
        /// Handle the Navigation Arguments
        /// Set the <see cref="_pageMode"/> if the Parameter is not <see cref="null"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var parameter = (Tuple<Mode, ProblemList, List<Problem>>)e.Parameter;
                _pageMode = parameter.Item1;
                _problemList = parameter.Item2;
                List<Problem> _problems = parameter.Item3;
                if (_pageMode == Mode.CreateProblemList && _problems != null)
                {
                    _problems.ForEach(problem => Problems.Add(problem));
                }
                if (_pageMode == Mode.EditProblemList && _problemList != null)
                {
                    _name = _problemList.Name;
                    _description = _problemList.Description;
                    _problemList.Problems.ToList().ForEach(problem => Problems.Add(problem));
                }
            }
            base.OnNavigatedTo(e);
        }

        private ObservableCollection<Problem> Problems = new() {  };

        /// <summary>
        /// When the text is changed in the search box, search in the database and create suggestion items. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void AddProblemBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrEmpty(AddProblemBox.Text))
                {
                    sender.ItemsSource = Problem.All;
                }
                else
                {
                    string keyword = AddProblemBox.Text.Trim();
                    List<Problem> resultList = new List<Problem>();
                    List<Problem> sourceList = Problem.All;
                    Problems.ToList().ForEach(problem => sourceList.Remove(problem));
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
                    sender.ItemsSource = resultList.ToList();
                }
            }
        }


        private void AddProblemBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            Problem.All.Where(problem => problem.Name == sender.Text).ToList().ForEach(problem => { if (!Problems.Contains(problem)) Problems.Add(problem); });
        }

        /// <summary>
        /// When the save button is clicked, save the problem list and change the description test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_pageMode == Mode.CreateProblemList)
            {
                ProblemList.Create(_name, _description, Problems.ToList());
            }
            else if (_pageMode == Mode.EditProblemList)
            {
                _problemList.Name = _name;
                _problemList.Description = _description;
                Problems.ToList().ForEach(problem =>
                {
                    if (_problemList.Problems.Contains(problem) == false)
                    {
                        _problemList.AddProblem(problem);
                    }
                });
                _problemList.Problems.ToList().ForEach(problem =>
                {
                    if (Problems.Contains(problem) == false)
                    {
                        _problemList.RemoveProblem(problem);
                    }
                });
            }
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
