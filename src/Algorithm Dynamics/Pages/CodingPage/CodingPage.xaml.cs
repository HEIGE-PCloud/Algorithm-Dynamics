using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages.CodingPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CodingPage : Page
    {
        public CodingPage()
        {
            InitializeComponent();
            ProblemNavView.SelectedItem = ProblemNavView.MenuItems[0];
            DataNavView.SelectedItem = DataNavView.MenuItems[0];
        }

        private void ProblemNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (ProblemNavView.SelectedItem == ProblemNavView.MenuItems[0])
            {
                ProblemFrame.NavigateToType(typeof(ProblemMarkdownPage), null, navOptions);
            }
            else if (ProblemNavView.SelectedItem == ProblemNavView.MenuItems[1])
            {
                ProblemFrame.NavigateToType(typeof(SubmissionsPage), null, navOptions);
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (DataNavView.SelectedItem == DataNavView.MenuItems[0])
            {
                DataFrame.NavigateToType(typeof(InputPanelPage), null, navOptions);
                //(DataFrame.Content as InputPanelPage).InputTextBox.Text = "OOOO";
            }
            else if (DataNavView.SelectedItem == DataNavView.MenuItems[1])
            {
                DataFrame.NavigateToType(typeof(OutputPanelPage), null, navOptions);
            }
            else if (DataNavView.SelectedItem == DataNavView.MenuItems[2])
            {
                DataFrame.NavigateToType(typeof(ErrorPanelPage), null, navOptions);
            }
        }

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            AppWindow window = MainWindow.AppWindow;
            if (window.Presenter.Kind == AppWindowPresenterKind.Overlapped)
            {
                window.SetPresenter(AppWindowPresenterKind.FullScreen);
                FullScreenIcon.Glyph = "\xE73F";
            }
            else
            {
                window.SetPresenter(AppWindowPresenterKind.Overlapped);
                FullScreenIcon.Glyph = "\xE740";
            }
        }
    }
}
