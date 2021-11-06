using Microsoft.UI.Xaml;
using Algorithm_Dynamics.Core.Models;
using Windows.Storage;
using System;
using System.Runtime.Versioning;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Algorithm_Dynamics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    [SupportedOSPlatform("windows10.0.10240.0")]
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            PrepareSourceCodeFile();
            m_window = new MainWindow();
            m_window.Activate();
        }

        private async void PrepareSourceCodeFile()
        {
            StorageFolder TemporaryFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile SourceCodeFile = await TemporaryFolder.CreateFileAsync("sol.txt", CreationCollisionOption.ReplaceExisting);
            Judger.SourceCodeFilePath = SourceCodeFile.Path;
            Judger.SourceCodeFolderPath = TemporaryFolder.Path;
            Judger.ExecutableFilePath = SourceCodeFile.Path.Replace("sol.txt", "sol.exe");
        }

        internal Window m_window;
    }
}
