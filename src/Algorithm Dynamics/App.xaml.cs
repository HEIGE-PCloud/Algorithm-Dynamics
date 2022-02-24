using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Algorithm_Dynamics.Pages;
using Algorithm_Dynamics.Core.Helpers;
using System.IO;

namespace Algorithm_Dynamics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Init Judger
            StorageFolder TemporaryFolder = ApplicationData.Current.TemporaryFolder;
            Judger.SetSourceCodeFilePath(TemporaryFolder.Path, "main");

            // Init Database
            StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
            DataAccess.InitializeDatabase(Path.Combine(LocalFolder.Path, "Data.db"));

            m_window = new MainWindow();

            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            var CurrentThemeValue = roamingSettings.Values["Theme"];
            ElementTheme theme;
            if (CurrentThemeValue != null)
            {
                theme = (ElementTheme)CurrentThemeValue;
            }
            else
            {
                theme = ElementTheme.Default;
                roamingSettings.Values["Theme"] = (int)ElementTheme.Default;
            }
            if (m_window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
            m_window.Activate();
        }

        public static MainWindow m_window;
        public static NavigationView MainNavView { get => m_window.MainNavView; }
        public static Frame ContentFrame { get => m_window.ContentFrame; }

        /// <summary>
        /// Handle the navigation of the main <see cref="ContentFrame"/>
        /// When the page is listed in the <see cref="MainNavView"/>, 
        /// change the <see cref="MainNavView.SelectedItem"/> directly.
        /// Otherwise, set the <see cref="MainNavView.SelectedItem"/> to null and 
        /// apply the navigation on the Frame directly.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parameter"></param>
        public static void NavigateTo(Type type, object parameter = null)
        {
            if (type == typeof(HomePage))
                MainNavView.SelectedItem = MainNavView.MenuItems[0];
            else if (type == typeof(ProblemsPage))
                MainNavView.SelectedItem = MainNavView.MenuItems[1];
            else if (type == typeof(AssignmentsPage))
                MainNavView.SelectedItem = MainNavView.MenuItems[2];
            else if (type == typeof(PlaygroundPage))
                MainNavView.SelectedItem = MainNavView.MenuItems[3];
            else if (type == typeof(AccountPage))
                MainNavView.SelectedItem = MainNavView.FooterMenuItems[0];
            else if (type == typeof(SettingsPage))
                MainNavView.SelectedItem = MainNavView.SettingsItem;
            else
            {
                ContentFrame.Navigate(type, parameter);
                MainNavView.SelectedItem = null;
            }
        }
        public static bool CanGoBack { get => ContentFrame.CanGoBack; }
    }
}
