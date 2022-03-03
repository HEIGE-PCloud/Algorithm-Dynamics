﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CreateNewProblemPage : Page
    {
        public CreateNewProblemPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<TestCase> TestCases = new() { new TestCase("", "", true) };
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
            }
            base.OnNavigatedTo(e);
        }
        /// <summary>
        /// Add a new <see cref="TestCase"/> to the list.
        /// Scroll the <see cref="scrollViewer"/> to the new position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestCase(object sender, RoutedEventArgs e)
        {
            TestCases.Add(new TestCase("", "", false));
            GeneralTransform transform = AddTestCaseButton.TransformToVisual((UIElement)scrollViewer.Content);
            Point position = transform.TransformPoint(new Point(0, 0));
            scrollViewer.ChangeView(null, position.Y, null, false);
        }

        /// <summary>
        /// Delete the selected <see cref="TestCase"/>. Update the <see cref="TestCase.Id"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSingleTestCase(object sender, RoutedEventArgs e)
        {
            TestCase selectedItem = ((FrameworkElement)sender).DataContext as TestCase;
            TestCases.Remove(selectedItem);
        }

        /// <summary>
        /// Delete all <see cref="TestCase"/>, hide the <see cref="DeleteConfirmFlyout"/>.
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
            // TODO: save the Problem to database
            App.NavigateTo(typeof(ProblemsPage));
        }
    }
    public class TestCase
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
        public TestCase(string input, string output, bool isExample)
        {
            Input = input;
            Output = output;
            IsExample = isExample;
        }
    }
}