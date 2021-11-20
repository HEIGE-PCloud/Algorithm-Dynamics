using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Input;
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
                ProblemListViewItemList.Add(new Problem(System.Guid.NewGuid(), $"Problem {i}", ProblemStatus.Todo, Difficulty.Easy, "", 0, 0, new List<TestCase> (), new List<Tag> { new Tag("Tag") }));
            }
            Lists.Add("List 1");
            Lists.Add("List 2");
            Lists.Add("List 3");
            Lists.Add("List 4");
        }
        private List<Problem> ProblemListViewItemList;
        private ObservableCollection<string> Lists = new();
        private ObservableCollection<string> Difficulties = new() { "Easy", "Medium", "Hard" };
        private ObservableCollection<string> Statuses = new() { "Todo", "Attempted", "Done" };

        private void ProblemsListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count == 1)
            {
                SingleSelectedMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            }
            else if (listView.SelectedItems.Count > 1)
            {
                MultipleSelectedMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            }
            if (listView.SelectedItems.Count > 0)
            {
                ProblemsSearchBox.Text = "";
                foreach (var item in listView.SelectedItems)
                {
                    Problem problem = item as Problem;
                    ProblemsSearchBox.Text += problem.Name + ", ";
                }
            }
        }

        private void ListComboBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListComboBox.SelectedIndex = -1;
        }
    }
}
