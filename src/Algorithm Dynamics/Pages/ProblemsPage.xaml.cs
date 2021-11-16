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
            for (int i = 1; i <= 100; i++)
            {
                ProblemListViewItemList.Add(new Problem($"Problem {i}"));
            }
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
