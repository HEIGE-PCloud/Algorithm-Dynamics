using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

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
            ProblemListViewItemList = new List<ProblemsListViewItem>();
            ProblemListViewItemList.Add(new ProblemsListViewItem("Problem A + B"));
        }
        private List<ProblemsListViewItem> ProblemListViewItemList;
        private void SelectProblemsToggleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
