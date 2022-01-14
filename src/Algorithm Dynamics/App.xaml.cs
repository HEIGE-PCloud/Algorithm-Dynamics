﻿using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

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
            StorageFolder TemporaryFolder = ApplicationData.Current.TemporaryFolder;
            Judger.SetSourceCodeFilePath(TemporaryFolder.Path, "main");
            m_window = new MainWindow();
            // Force a light theme for screenshots
            if (m_window.Content is FrameworkElement rootElement)
            {
                //rootElement.RequestedTheme = ElementTheme.Light;
            }
            m_window.Activate();
        }

        public MainWindow m_window;
        public NavigationView MainNavView { get => m_window.MainNavView; }
        public Frame ContentFrame { get => m_window.ContentFrame; }
    }
}