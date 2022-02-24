using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Reflection;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            GetCurrentTheme();
        }
        private const int DEFAULT_RUN_CODE_TIMELIMIT = 1000;
        private const int DEFAULT_RUN_CODE_MEMORYLIMIT = 64 * 1024 * 1024;
        public int TimeLimit
        {
            get
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                var CurrentValue = roamingSettings.Values["RunCodeMemoryLimit"];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue;
                }
                else
                {
                    roamingSettings.Values["RunCodeMemoryLimit"] = DEFAULT_RUN_CODE_MEMORYLIMIT;
                    return DEFAULT_RUN_CODE_MEMORYLIMIT;
                }
            }
            set
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["RunCodeMemoryLimit"] = value;

            }
        }
        public int MemoryLimit
        {
            get
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                var CurrentValue = roamingSettings.Values["RunCodeMemoryLimit"];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue;
                }
                else
                {
                    roamingSettings.Values["RunCodeMemoryLimit"] = DEFAULT_RUN_CODE_TIMELIMIT;
                    return DEFAULT_RUN_CODE_TIMELIMIT;
                }
            }
            set
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["RunCodeMemoryLimit"] = value;

            }
        }
        private async void AddLangButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void GetCurrentTheme()
        {
            if (App.m_window.Content is FrameworkElement rootElement)
            {
                var currentTheme = rootElement.RequestedTheme.ToString();
                ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
            }
        }

        /// <summary>
        /// Change the application request theme to the selected theme when the radio button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
