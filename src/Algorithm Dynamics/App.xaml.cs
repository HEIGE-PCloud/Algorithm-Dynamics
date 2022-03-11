using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO;
using Windows.Storage;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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
            AppCenter.Start("743693e0-2286-4cf0-9a16-a448c1059eed", typeof(Analytics), typeof(Crashes));
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
            // C:\Users\pcloud\AppData\Local\Packages\55445HEIGE-PCloud.AlgorithmDynamics_r55vz1why9y6a\LocalState
            StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
            DataAccess.InitializeDatabase(Path.Combine(LocalFolder.Path, "Data.db"));

            // Create main window
            m_window = new MainWindow();

            // Load theme
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            var CurrentThemeValue = roamingSettings.Values["Theme"];
            // if there exists a theme setting, use the theme setting
            // otherwise set the default theme and save it to the setting
            ElementTheme theme;
            if (CurrentThemeValue != null)
            {
                theme = (ElementTheme)CurrentThemeValue;
            }
            else
            {
                theme = ElementTheme.Default;
                roamingSettings.Values["Theme"] = (int)theme;
            }
            if (m_window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }

            if (Language.All.Count == 0) InitializeLanguageConfiguration();


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
                MainNavView.SelectedItem = MainNavView.MenuItems[3];
            else if (type == typeof(PlaygroundPage))
                MainNavView.SelectedItem = MainNavView.MenuItems[2];
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

        public static User CurrentUser
        {
            get
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                Guid uid = (Guid)roamingSettings.Values["CurrentUser"];
                return User.Get(uid);
            }
        }
        private static void InitializeLanguageConfiguration()
        {
            Language.Create("python", "Python", false, "", "", "python.exe", "{SourceCodeFilePath}", ".py");
            Language.Create("c", "C", true, "gcc.exe", "-x c {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".c");
            Language.Create("cpp", "C++", true, "g++.exe", "-x c++ {SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".cpp");
            Language.Create("rust", "Rust", true, "rustc.exe", "{SourceCodeFilePath} -o {ExecutableFilePath}", "{ExecutableFilePath}", "", ".rs");
            Language.Create("javascript", "JavaScript", false, "", "", "node.exe", "{SourceCodeFilePath}", ".js");
            Language.Create("java", "Java", true, "javac.exe", "{SourceCodeFilePath}", "java.exe", "main", ".java");
            Language.Create("go", "Go", true, "go.exe", "build {SourceCodeFilePath}", "{ExecutableFilePath}", "", ".go");
        }

    }
}
