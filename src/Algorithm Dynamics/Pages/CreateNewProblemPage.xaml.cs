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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateNewProblemPage : Page
    {
        public CreateNewProblemPage()
        {
            InitializeComponent();
        }
        public ObservableCollection<TestCase> TestCases = new() { };

        private void DeleteSingleTestCase(object sender, RoutedEventArgs e)
        {
            TestCase selectedItem = ((FrameworkElement)sender).DataContext as TestCase;
            TestCases.Remove(selectedItem);

        }

        private void AddTestCase(object sender, RoutedEventArgs e)
        {
            TestCases.Add(new TestCase(1, "", "", false));
        }
    }
    public class TestCase
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
        public TestCase(int id, string input, string output, bool isExample)
        {
            Id = id;
            Input = input;
            Output = output;
            IsExample = isExample;
        }
    }
}
