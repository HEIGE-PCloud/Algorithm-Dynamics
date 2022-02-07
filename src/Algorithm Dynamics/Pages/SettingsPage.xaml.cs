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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            GetCurrentTheme();
        }

        private async void AddLangButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Add a new Programming Language Configuration";
            dialog.PrimaryButtonText = "Add";
            dialog.CloseButtonText = "Cancel";
            dialog.Content = "Content";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.XamlRoot = this.Content.XamlRoot;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // TODO: Delete the selected problem list
                throw new NotImplementedException();
            }
        }
        private void GetCurrentTheme()
        {
            if (App.m_window.Content is FrameworkElement rootElement)
            {
                var currentTheme = rootElement.RequestedTheme.ToString();
                ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
            }
        }
        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

            if (selectedTheme != null && App.m_window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = GetEnum<ElementTheme>(selectedTheme);
            }
        }
        private static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
