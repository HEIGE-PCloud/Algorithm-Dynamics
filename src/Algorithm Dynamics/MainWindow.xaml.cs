using Algorithm_Dynamics.Helpers;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Algorithm_Dynamics
{
    /// <summary>
    /// The main window object that is used to display all other elements
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static AppWindow AppWindow;
        public MainWindow()
        {
            InitializeComponent();
            AppWindow = AppWindowExtensions.GetAppWindow(this);
            Title = "Algorithm Dynamics";
            // Select HomePage when first loaded
            MainNavView.SelectedItem = MainNavView.MenuItems[0];
        }

        /// <summary>
        /// Handle the SelectionChanged event of the MainNavView
        /// Navigate to the corresponding page when the selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // If the settings is selected, navigate to the settings page
                ContentFrame.Navigate(typeof(Pages.SettingsPage));
            }
            else
            {
                // Otherwise, get the selected item. If it is not null, get its Tag
                // Navigate to Algorithm_Dynamics.Pages.<Tag>
                NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
                if (selectedItem != null)
                {
                    string tag = selectedItem.Tag as string;
                    string pageName = "Algorithm_Dynamics.Pages." + tag;
                    Type pageType = Type.GetType(pageName);
                    ContentFrame.Navigate(pageType);
                }
            }
        }
        public void NavigateTo()
        {

        }

        private void MainNavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack) ContentFrame.GoBack();
        }
    }
}
