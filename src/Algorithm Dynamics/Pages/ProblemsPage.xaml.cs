using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProblemsPage : Page
    {
        public ProblemsPage()
        {
            this.InitializeComponent();
            ProblemListViewItemList = new List<Problem>();
            ProblemListViewItemList.Add(new Problem("Problem 1"));
            ProblemListViewItemList.Add(new Problem("Problem 2"));
            ProblemListViewItemList.Add(new Problem("Problem 3"));
            ProblemListViewItemList.Add(new Problem("Problem 4"));
            ProblemListViewItemList.Add(new Problem("Problem 5"));
            ProblemListViewItemList.Add(new Problem("Problem 6"));
            ProblemListViewItemList.Add(new Problem("Problem 7"));
            ProblemListViewItemList.Add(new Problem("Problem 8"));
            ProblemListViewItemList.Add(new Problem("Problem 9"));
            ProblemListViewItemList.Add(new Problem("Problem 10"));
            ProblemListViewItemList.Add(new Problem("Problem 11"));
            ProblemListViewItemList.Add(new Problem("Problem 12"));
            ProblemListViewItemList.Add(new Problem("Problem 13"));
            ProblemListViewItemList.Add(new Problem("Problem 14"));
            ProblemListViewItemList.Add(new Problem("Problem 15"));
            ProblemListViewItemList.Add(new Problem("Problem 16"));
            ProblemListViewItemList.Add(new Problem("Problem 17"));
            ProblemListViewItemList.Add(new Problem("Problem 18"));
            ProblemListViewItemList.Add(new Problem("Problem 19"));
            ProblemListViewItemList.Add(new Problem("Problem 20"));
            ProblemListViewItemList.Add(new Problem("Problem 1"));
            ProblemListViewItemList.Add(new Problem("Problem 2"));
            ProblemListViewItemList.Add(new Problem("Problem 3"));
            ProblemListViewItemList.Add(new Problem("Problem 4"));
            ProblemListViewItemList.Add(new Problem("Problem 5"));
            ProblemListViewItemList.Add(new Problem("Problem 6"));
            ProblemListViewItemList.Add(new Problem("Problem 7"));
            ProblemListViewItemList.Add(new Problem("Problem 8"));
            ProblemListViewItemList.Add(new Problem("Problem 9"));
            ProblemListViewItemList.Add(new Problem("Problem 10"));
            ProblemListViewItemList.Add(new Problem("Problem 11"));
            ProblemListViewItemList.Add(new Problem("Problem 12"));
            ProblemListViewItemList.Add(new Problem("Problem 13"));
            ProblemListViewItemList.Add(new Problem("Problem 14"));
            ProblemListViewItemList.Add(new Problem("Problem 15"));
            ProblemListViewItemList.Add(new Problem("Problem 16"));
            ProblemListViewItemList.Add(new Problem("Problem 17"));
            ProblemListViewItemList.Add(new Problem("Problem 18"));
            ProblemListViewItemList.Add(new Problem("Problem 19"));
            ProblemListViewItemList.Add(new Problem("Problem 20"));
            Lists.Add("List 1");
            Lists.Add("List 2");
            Lists.Add("List 3");
            Lists.Add("List 4");
        }
        private List<Problem> ProblemListViewItemList;
        private ObservableCollection<string> Lists = new();
        private void SelectProblemsToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
