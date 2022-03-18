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
using Windows.ApplicationModel;
using WinRT.Interop;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Algorithm_Dynamics.Helpers;
using System.Threading.Tasks;

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
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            InitializeJudger();
            InitializeDatabase();
            InitializeLanguageConfiguration();
            await InitializeSampleProblems();
            InitializeMainWindow();
            InitializeTheme();
        }

        /// <summary>
        /// Initialize the drag area of the title bar
        /// The left-most 40px is used for the go back button
        /// </summary>
        private static void InitializeDragArea()
        {
            RectInt32 rect = new(40, 0, AppWindowExtensions.GetScalePixel(_appWindow.Size.Width, _windowHandle), AppWindowExtensions.GetScalePixel(36, _windowHandle));
            _appWindow.TitleBar.SetDragRectangles(new RectInt32[] { rect });
        }

        /// <summary>
        /// Initialize the Judger at temporary folder
        /// </summary>
        private static void InitializeJudger()
        {
            StorageFolder TemporaryFolder = ApplicationData.Current.TemporaryFolder;
            Judger.SetSourceCodeFilePath(TemporaryFolder.Path, "main");
        }

        /// <summary>
        /// Initialize the database at local folder
        /// </summary>
        private static void InitializeDatabase()
        {
            // Init Database
            // C:\Users\pcloud\AppData\Local\Packages\55445HEIGE-PCloud.AlgorithmDynamics_r55vz1why9y6a\LocalState
            StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
            DataAccess.InitializeDatabase(Path.Combine(LocalFolder.Path, "Data.db"));

        }

        /// <summary>
        /// Initialize the App theme from the local setting
        /// </summary>
        private static void InitializeTheme()
        {
            // Load theme
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var CurrentThemeValue = localSettings.Values["Theme"];

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
                localSettings.Values["Theme"] = (int)theme;
            }

            // Apply theme to rootElement
            if (m_window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        /// <summary>
        /// Add default language config to the database
        /// when there is no language config in database
        /// </summary>
        private static void InitializeLanguageConfiguration()
        {
            if (Language.All.Count == 0)
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

        /// <summary>
        /// Add sample problems to the database
        /// when there is no problem in the database
        /// </summary>
        private async static Task InitializeSampleProblems()
        {
            if (Problem.All.Count == 0)
            {
                StorageFolder AssetsDirectory = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
                string helloWorld = await File.ReadAllTextAsync(Path.Combine(AssetsDirectory.Path, "Problems\\Hello World Export.json"));
                DataSerialization.DeserializeProblem(helloWorld);
                string APlusB = await File.ReadAllTextAsync(Path.Combine(AssetsDirectory.Path, "Problems\\A + B Problem Export.json"));
                DataSerialization.DeserializeProblem(APlusB);
            }
        }

        /// <summary>
        /// Init main window and title bar
        /// </summary>
        private static void InitializeMainWindow()
        {
            // Create main window
            m_window = new MainWindow();

            // Set App title
            _windowHandle = WindowNative.GetWindowHandle(m_window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(_windowHandle);
            _appWindow = AppWindow.GetFromWindowId(windowId);
            _appWindow.Title = "Algorithm Dynamics";

            // Init title bar
            if (_appWindow.TitleBar != null)
            {
                AppWindowExtensions.InitializeTitleBar(_appWindow.TitleBar);
                InitializeDragArea();
            }

            // Activate main window
            m_window.Activate();
        }

        private static AppWindow _appWindow;
        private static IntPtr _windowHandle;
        private static NavigationView MainNavView { get => m_window.MainNavView; }
        private static Frame ContentFrame { get => m_window.ContentFrame; }
        public static MainWindow m_window;

        /// <summary>
        /// Get the current user.
        /// </summary>
        public static User CurrentUser
        {
            get
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                Guid uid = (Guid)localSettings.Values["CurrentUser"];
                return User.Get(uid);
            }
        }

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
    }
}
