using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentsPage : Page
    {
        public AssignmentsPage()
        {
            this.InitializeComponent();
            AssignmentsNavigationView.SelectedItem = AssignmentsNavigationView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void AssignmentsNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                string selectedItemTag = selectedItem.Tag as string;
                string pageName = "Algorithm_Dynamics.Pages." + selectedItemTag + "Page";
                Type pageType = Type.GetType(pageName);
                AssignmentsFrame.Navigate(pageType);
            }
        }
    }
}
