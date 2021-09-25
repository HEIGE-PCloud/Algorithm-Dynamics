using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Pages.SettingsPage));
            }
            else
            {
                NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
                if (selectedItem != null)
                {
                    string selectedItemTag = selectedItem.Tag as string;
                    //sender.Header = "Sample Page " + selectedItemTag.Substring(selectedItemTag.Length - 1);
                    string pageName = "Algorithm_Dynamics.Pages." + selectedItemTag;
                    Type pageType = Type.GetType(pageName);
                    contentFrame.Navigate(pageType);
                }
            }
        }
        public void SetSelectedNavigationItem(int index)
        {
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[index];
        }
        public void SetSelectedNavigationItem(NavigationViewItem item)
        {
            MainNavigationView.SelectedItem = item;
        }
    }
}
