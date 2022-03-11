using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Foundation;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CreateNewProblemPage : Page, INotifyPropertyChanged
    {
        public CreateNewProblemPage()
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/901921/observablecollection-and-item-propertychanged
            TestCases.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged item in e.OldItems)
                        item.PropertyChanged -= PrimitiveTestCase_PropertyChanged;
                }
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged item in e.NewItems)
                        item.PropertyChanged += PrimitiveTestCase_PropertyChanged;
                }
            };
        }

        /// <summary>
        /// Update the <see cref="DescriptionMarkdown"/> when any <see cref="PrimitiveTestCase"/> has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrimitiveTestCase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DescriptionMarkdown));
        }

        /// <summary>
        /// Set the value for a property and invoke <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="newValue"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke a new <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Use <see cref="nameof"/> to get the name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DescriptionMarkdown)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValidInput)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
        }

        public enum Mode
        {
            Create,
            Edit
        }

        private Mode _pageMode = Mode.Create;
        private string Title
        {
            get
            {
                if (_pageMode == Mode.Create)
                    return "Create Problem";
                else
                    return "Edit Problem";
            }
        }
        private Problem _problem;
        private string _name = "";
        private string _tags = "";
        private string _description = @"## Description

A detailed description of the problem.

(Calculate A + B for two integers.)

## Input

The format and the range of the input.

(Two integers A and B, separated by a space. 0 <= A, B <= 10000.)

## Output

The format and the range of the output

(A single integer, the sum of A and B.)

";
        private int _timeLimit = 1000;
        private int _memoryLimit = 64;

        public ObservableCollection<PrimitiveTestCase> TestCases = new();
        public string ProblemName { get => _name; set => Set(ref _name, value); }
        public string Tags { get => _tags; set => Set(ref _tags, value); }
        public string Description { get => _description; set => Set(ref _description, value); }
        public int TimeLimit { get => _timeLimit; set => Set(ref _timeLimit, value); }
        public int MemoryLimit { get => _memoryLimit; set => Set(ref _memoryLimit, value); }
        public int Difficulty { get => _difficulty; set => Set(ref _difficulty, value); }
        private int _difficulty = (int)Core.Models.Difficulty.Easy;

        public string DescriptionMarkdown
        {
            get
            {
                string timeLimit = $"\n## Time Limit\n\n{_timeLimit} ms";
                string memoryLimit = $"\n## Memory Limit\n\n{_memoryLimit} MB";
                string example = "\n## Example";
                int testCaseCnt = 1;
                TestCases.Where(testCase => testCase.IsExample == true).ToList().ForEach(testCase =>
                {
                    example += $"\n### Example Input {testCaseCnt}\n";
                    example += "```\n" + testCase.Input.Replace("\n", "\n\n") + "\n```\n";
                    example += $"\n### Example Output {testCaseCnt}\n";
                    example += "```\n" + testCase.Output.Replace("\n", "\n\n") + "\n```\n";
                    testCaseCnt++;
                });
                return _description + timeLimit + memoryLimit + example;
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
                if (!TestCases.Any(testCase => testCase.IsExample == true))
                {
                    isValid = false;
                    ErrorMessage += "At least one example test case is required.\n";
                }
                if (!TestCases.Any(testCase => testCase.IsExample == false))
                {
                    isValid = false;
                    ErrorMessage += "At least one non-example test case is required.\n";
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
                var parameter = (Tuple<Mode, Problem>)e.Parameter;
                _pageMode = parameter.Item1;
                _problem = parameter.Item2;
                if (_pageMode == Mode.Edit)
                {
                    _name = _problem.Name;
                    _description = _problem.Description;
                    _timeLimit = _problem.TimeLimit;
                    _memoryLimit = (int)_problem.MemoryLimit / 1024 / 1024;
                    _difficulty = (int)_problem.Difficulty;
                    foreach (var t in _problem.TestCases)
                    {
                        TestCases.Add(new PrimitiveTestCase(t.Input, t.Output, t.IsExample));
                    }
                    _tags = _problem.TagsAsString;
                }
                else
                {
                    TestCases.Add(new PrimitiveTestCase("", "", true));
                    TestCases.Add(new PrimitiveTestCase("", "", false));
                }
            }
            else
            {
                TestCases.Add(new PrimitiveTestCase("", "", true));
                TestCases.Add(new PrimitiveTestCase("", "", false));
            }
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Add a new <see cref="PrimitiveTestCase"/> to the list.
        /// Scroll the <see cref="scrollViewer"/> to the new position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestCase(object sender, RoutedEventArgs e)
        {
            TestCases.Add(new PrimitiveTestCase("", "", false));
            GeneralTransform transform = AddTestCaseButton.TransformToVisual((UIElement)scrollViewer.Content);
            Point position = transform.TransformPoint(new Point(0, 0));
            scrollViewer.ChangeView(null, position.Y, null, false);
        }

        /// <summary>
        /// Delete the selected <see cref="PrimitiveTestCase"/>. Update the <see cref="PrimitiveTestCase.Id"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSingleTestCase(object sender, RoutedEventArgs e)
        {
            PrimitiveTestCase selectedItem = ((FrameworkElement)sender).DataContext as PrimitiveTestCase;
            TestCases.Remove(selectedItem);
        }

        /// <summary>
        /// Delete all <see cref="PrimitiveTestCase"/>, hide the <see cref="DeleteConfirmFlyout"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllTestCases(object sender, RoutedEventArgs e)
        {
            TestCases.Clear();
            DeleteConfirmFlyout.Hide();
        }

        /// <summary>
        /// Cancel the creation, navigate back to the <see cref="ProblemsPage"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelCreation(object sender, RoutedEventArgs e)
        {
            CancelConfirmFlyout.Hide();
            App.NavigateTo(typeof(ProblemsPage));
        }

        /// <summary>
        /// Create the problem and save it into the database.
        /// Navigate back to the <see cref="ProblemsPage"/> afterwards.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateProblem(object sender, RoutedEventArgs e)
        {
            if (_pageMode == Mode.Create)
            {
                // Create tag only when there is input
                List<Tag> tags = null;
                if (string.IsNullOrWhiteSpace(_tags) == false)
                {
                    tags = new();
                    foreach (var t in _tags.Split(',').ToList())
                    {
                        tags.Add(Core.Models.Tag.Create(t.Trim()));
                    }
                }

                // Create test cases
                List<TestCase> testCases = new();
                foreach (var t in TestCases)
                {
                    testCases.Add(TestCase.Create(t.Input, t.Output, t.IsExample));
                }

                // Create Problem
                Problem.Create(Guid.NewGuid(), _name, _description, _timeLimit, _memoryLimit * 1024 * 1024, (Difficulty)_difficulty, testCases, tags);
            }
            else
            {
                _problem.Name = _name;
                _problem.Description = _description;
                _problem.TimeLimit = _timeLimit;
                _problem.MemoryLimit = _memoryLimit * 1024 * 1024;
                _problem.Difficulty = (Difficulty)_difficulty;
                while (_problem.Tags.Count != 0) _problem.RemoveTag(_problem.Tags[0]);
                while (_problem.TestCases.Count != 0) _problem.RemoveTestCase(_problem.TestCases[0]);
                // Create tag
                foreach (var t in _tags.Split(',').ToList())
                {
                    _problem.AddTag(Core.Models.Tag.Create(t.Trim()));
                }

                // Create test cases
                foreach (var t in TestCases)
                {
                    _problem.AddTestCase(TestCase.Create(t.Input, t.Output, t.IsExample));
                }
            }
            App.NavigateTo(typeof(ProblemsPage));
        }
    }

    public class PrimitiveTestCase : INotifyPropertyChanged
    {
        private string _input;
        private string _output;
        private bool _isExample;
        public string Input
        {
            get => _input;
            set
            {
                if (_input != value)
                {
                    _input = value;
                    OnPropertyChanged(nameof(Input));
                }
            }
        }
        public string Output
        {
            get => _output;
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged(nameof(Output));
                }
            }
        }
        public bool IsExample
        {
            get => _isExample;
            set
            {
                if (_isExample != value)
                {
                    _isExample = value;
                    OnPropertyChanged(nameof(IsExample));
                }
            }
        }
        public PrimitiveTestCase(string input, string output, bool isExample)
        {
            Input = input;
            Output = output;
            IsExample = isExample;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
