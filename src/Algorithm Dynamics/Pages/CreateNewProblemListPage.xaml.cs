using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage.Pickers;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CreateNewProblemListPage : Page, INotifyPropertyChanged
    {
        public CreateNewProblemListPage()
        {
            InitializeComponent();
            Problems.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                OnPropertyChanged(nameof(Problems));
            };
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
        public string ListName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(ListName));
                }
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public bool IsValidInput
        {
            get
            {
                ErrorMessage = "";
                bool isValid = true;
                if (string.IsNullOrEmpty(_name))
                {
                    isValid = false;
                    ErrorMessage += "A name is required.\n";
                }
                if (Problems.Count < 1)
                {
                    isValid = false;
                    ErrorMessage += "At least one problem is required.\n";
                }
                return isValid;
            }
        }
        public string ErrorMessage { get; set; }

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

        private ObservableCollection<Problem> Problems = new() { };

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValidInput)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
        }

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
                _problemList = ProblemList.Create(_name, _description, Problems.ToList());
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
            string fileName = $"{_problemList.Name} Export";
            string serializedProblem = DataSerialization.SerializeProblemList(_problemList);
            bool success = await FileHelper.FileSavePicker("Algorithm Dynamics Export File", new() { ".json" }, fileName, serializedProblem);
            if (success) Finish(null, null);
        }

    }
}
