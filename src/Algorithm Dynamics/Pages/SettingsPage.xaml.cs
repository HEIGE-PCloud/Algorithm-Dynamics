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
            InitializeLanguage();
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Language> Languages { get; set; } = new();

        /// <summary>
        /// The RunCode TimeLimit value
        /// </summary>
        public static int TimeLimit
        {
            get
            {
                // Read time limit from settings
                // If does not exist, store the default time limit
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                var CurrentValue = localSettings.Values[TIMELIMIT_KEY];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue;
                }
                else
                {
                    localSettings.Values[TIMELIMIT_KEY] = DEFAULT_RUN_CODE_TIMELIMIT;
                    return DEFAULT_RUN_CODE_TIMELIMIT;
                }
            }
            set
            {
                // Update the setting
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values[TIMELIMIT_KEY] = value;
            }
        }

        /// <summary>
        /// The RunCode MemoryLimit value
        /// </summary>
        public static int MemoryLimit
        {
            get
            {
                // Read memory limit from settings
                // If does not exist, store the default memory limit
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                var CurrentValue = localSettings.Values[MEMORYLIMIT_KEY];
                if (CurrentValue != null)
                {
                    return (int)CurrentValue / MB;
                }
                else
                {
                    localSettings.Values[MEMORYLIMIT_KEY] = DEFAULT_RUN_CODE_MEMORYLIMIT;
                    return DEFAULT_RUN_CODE_MEMORYLIMIT / MB;
                }
            }
            set
            {
                // Update the setting
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values[MEMORYLIMIT_KEY] = value * MB;
            }
        }

        /// <summary>
        /// Get the current theme and set the check box
        /// </summary>
        private void GetCurrentTheme()
        {
            if (App.m_window.Content is FrameworkElement rootElement)
            {
                string currentTheme = rootElement.RequestedTheme.ToString();
                ThemePanel
                    .Children
                    .Cast<RadioButton>()
                    .FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)
                    .IsChecked = true;
            }
        }

        /// <summary>
        /// Initialize all language configs from the database
        /// </summary>
        private void InitializeLanguage()
        {
            Core.Models.Language.All.ForEach(lang => Languages.Add(lang));
        }

        private void AddLangButton_Click(object sender, RoutedEventArgs e)
        {
            LanguageComboBox.SelectedIndex = -1;
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
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["Theme"] = (int)rootElement.RequestedTheme;
            }
        }

        /// <summary>
        /// Get Enum item from string
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static TEnum GetEnum<TEnum>(string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }

        /// <summary>
        /// Save the edits to the language to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (LanguageComboBox.SelectedIndex == -1)
            {
                // Create a new language
                var lang = Core.Models.Language.Create(_name, _displayName, _needCompile, _complieCommand, _compileArguments, _runCommand, _runArguments, _fileExtension);
                Languages.Add(lang);
                LanguageComboBox.SelectedItem = lang;
            }
            else
            {
                // Edit an existing language
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

        /// <summary>
        /// Change the language config when the selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Delete the selected language config from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLanguageConfig(object sender, RoutedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem != null)
            {
                // Delete the language from database
                var lang = LanguageComboBox.SelectedItem as Language;
                Languages.Remove(lang);
                lang.Delete();

                // Reset the combo box selection to 0
                if (Languages.Count > 0)
                    LanguageComboBox.SelectedIndex = 0;
            }
            DeleteLangFlyout.Hide();
        }

        /// <summary>
        /// Delete all data in the database and quit the app
        /// TODO: https://portal.productboard.com/winappsdk/1-windows-app-sdk/c/54-restart-for-all-desktop-apps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllData(object sender, RoutedEventArgs e)
        {
            DataAccess.DropDatabase();
            App.Current.Exit();
        }

        /// <summary>
        /// Delete all problems, test cases, problem lists and sbmissions in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllProblems(object sender, RoutedEventArgs e)
        {
            ProblemList.All.ForEach(problemList => problemList.Delete());
            Problem.All.ForEach(problem => problem.Delete());
            ClearAllProblemsFlyout.Hide();
        }


        /// <summary>
        /// Delete all submissions and submission results in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllSubmissions(object sender, RoutedEventArgs e)
        {
            Submission.All.ForEach(submission => submission.Delete());
            ClearAllSubmissionsFlyout.Hide();            
        }
    }
}
