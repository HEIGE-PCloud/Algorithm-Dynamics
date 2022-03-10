using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        public SettingsPage()
        {
            InitializeComponent();

            GetCurrentTheme();

            Core.Models.Language.All.ForEach(lang => Languages.Add(lang));
        }
        const int MB = 1024 * 1024;
        private const int DEFAULT_RUN_CODE_TIMELIMIT = 1000;
        private const int DEFAULT_RUN_CODE_MEMORYLIMIT = 64 * MB;
        private const string TIMELIMIT_KEY = "RunCodeTimeLimit";
        private const string MEMORYLIMIT_KEY = "RunCodeMemoryLimit";
        private string _displayName = "";
        private string _name = "";
        private bool _needCompile = false;
        private string _complieCommand = "";
        private string _compileArguments = "";
        private string _runCommand = "";
        private string _runArguments = "";
        private string _fileExtension = "";

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Invoke a new <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Use <see cref="nameof"/> to get the name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Language> Languages { get; set; } = new();
        public int TimeLimit
        {
            get
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                var CurrentValue = roamingSettings.Values[TIMELIMIT_KEY];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue;
                }
                else
                {
                    roamingSettings.Values[TIMELIMIT_KEY] = DEFAULT_RUN_CODE_TIMELIMIT;
                    return DEFAULT_RUN_CODE_TIMELIMIT;
                }
            }
            set
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values[TIMELIMIT_KEY] = value;

            }
        }
        public int MemoryLimit
        {
            get
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                var CurrentValue = roamingSettings.Values[MEMORYLIMIT_KEY];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue / MB;
                }
                else
                {
                    roamingSettings.Values[MEMORYLIMIT_KEY] = DEFAULT_RUN_CODE_MEMORYLIMIT;
                    return DEFAULT_RUN_CODE_MEMORYLIMIT / MB;
                }
            }
            set
            {
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values[MEMORYLIMIT_KEY] = value * MB;
            }
        }
        private void AddLangButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageComboBox.SelectedIndex = -1;
        }
        private void GetCurrentTheme()
        {
            if (App.m_window.Content is FrameworkElement rootElement)
            {
                string currentTheme = rootElement.RequestedTheme.ToString();
                ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
            }
        }

        /// <summary>
        /// Change the application request theme to the selected theme when the radio button is clicked
        /// Save the theme into settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

            if (selectedTheme != null && App.m_window.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = GetEnum<ElementTheme>(selectedTheme);
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["Theme"] = (int)rootElement.RequestedTheme;
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

        private void SaveLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (LanguageComboBox.SelectedIndex == -1)
            {
                // Create a new one
                var lang = Core.Models.Language.Create(_name, _displayName, _needCompile, _complieCommand, _compileArguments, _runCommand, _runArguments, _fileExtension);
                Languages.Add(lang);
                LanguageComboBox.SelectedItem = lang;
            }
            else
            {
                // Edit an existing one
                var lang = LanguageComboBox.SelectedItem as Language;
                lang.Name = _name;
                lang.DisplayName = _displayName;
                lang.NeedCompile = _needCompile;
                lang.CompileCommand = _complieCommand;
                lang.CompileArguments = _compileArguments;
                lang.RunCommand = _runCommand;
                lang.RunArguments = _runArguments;
                lang.FileExtension = _fileExtension;
            }

        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lang = LanguageComboBox.SelectedItem as Core.Models.Language;
            if (lang != null)
            {
                _name = lang.Name;
                _displayName = lang.DisplayName;
                _needCompile = lang.NeedCompile;
                _complieCommand = lang.CompileCommand;
                _compileArguments = lang.CompileArguments;
                _runCommand = lang.RunCommand;
                _runArguments = lang.RunArguments;
                _fileExtension = lang.FileExtension;
            }
            else
            {
                _name = "";
                _displayName = "";
                _needCompile = false;
                _complieCommand = "";
                _compileArguments = "";
                _runCommand = "";
                _runArguments = "";
                _fileExtension = "";
            }
            OnPropertyChanged(nameof(_name));
            OnPropertyChanged(nameof(_displayName));
            OnPropertyChanged(nameof(_needCompile));
            OnPropertyChanged(nameof(_complieCommand));
            OnPropertyChanged(nameof(_compileArguments));
            OnPropertyChanged(nameof(_runCommand));
            OnPropertyChanged(nameof(_runArguments));
            OnPropertyChanged(nameof(_fileExtension));

        }

        private void DeleteLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem != null)
            {
                var lang = LanguageComboBox.SelectedItem as Language;
                Languages.Remove(lang);
                lang.Delete();
                if (Languages.Count > 0)
                    LanguageComboBox.SelectedIndex = 0;
            }
        }

        private void ClearAllData(object sender, RoutedEventArgs e)
        {
            DataAccess.DropDatabase();
            App.Current.Exit();
        }
    }
}
