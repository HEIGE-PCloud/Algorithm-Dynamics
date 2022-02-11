using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CreateNewProblemPage : Page, INotifyPropertyChanged
    {
        public CreateNewProblemPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<TestCase> TestCases = new() { new TestCase(1, "", "", true) };
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public enum Mode
        {
            Create,
            Edit
        }
        public Mode PageMode { get; set; } = Mode.Create;
        public string Title
        {
            get
            {
                if (PageMode == Mode.Create)
                {
                    return "Create Problem";
                }
                else
                {
                    return "Edit Problem";
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                PageMode = ((Tuple<Mode, int>)e.Parameter).Item1;
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
            TestCases.Add(new TestCase(TestCases.Count + 1, "", "", false));
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
            //TestCases.Remove(selectedItem);
            TestCases[0].Id = -1;
            for (int i = 0; i < TestCases.Count; i++)
                TestCases[i].Id = i + 1;
            System.Diagnostics.Debug.WriteLine(TestCases[0].Id);
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
    public class TestCase : INotifyPropertyChanged
    {
        private int _id;
        public int Id 
        { 
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
        public TestCase(int id, string input, string output, bool isExample)
        {
            _id = id;
            Input = input;
            Output = output;
            IsExample = isExample;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
