using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CreateNewProblemPage : Page
    {
        public CreateNewProblemPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<PrimitiveTestCase> TestCases = new();
        public enum Mode
        {
            Create,
            Edit
        }
        private Mode _pageMode = Mode.Create;
        private string _title
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
        private string _description = "";
        private int _timeLimit = 1000;
        private int _memoryLimit = 64;
        private int _difficulty = (int)Difficulty.Easy;
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
                    _tags = _problem.str_Tags;
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
                // Create tag
                List<Tag> tags = new();
                foreach (var t in _tags.Split(',').ToList())
                {
                    tags.Add(Core.Models.Tag.Create(t.Trim()));
                }

                // Create test cases
                List<TestCase> testCases = new();
                foreach (var t in TestCases)
                {
                    testCases.Add(TestCase.Create(t.Input, t.Output, t.IsExample));
                }

                // Create Problem
                Problem.Create(_name, _description, _timeLimit, _memoryLimit * 1024 * 1024, (Difficulty)_difficulty, testCases, tags);
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
    public class PrimitiveTestCase
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
        public PrimitiveTestCase(string input, string output, bool isExample)
        {
            Input = input;
            Output = output;
            IsExample = isExample;
        }
    }
}
