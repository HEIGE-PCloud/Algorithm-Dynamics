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

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class CodingPage : Page
    {
        public string Code { get; set; }
        public CodingPage()
        {
            InitializeComponent();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CodeEditor.Lang = LanguageComboBox.SelectedItem as string;
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
