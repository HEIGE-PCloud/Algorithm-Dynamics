using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Helpers;
using Algorithm_Dynamics.Pages;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Storage;

namespace Algorithm_Dynamics
{
    /// <summary>
    /// The main window object that is used to display all other elements
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static AppWindow AppWindow;
        public MainWindow()
        {
            InitializeComponent();
            AppWindow = AppWindowExtensions.GetAppWindow(this);
            Title = "Algorithm Dynamics";

            if (DataAccess.GetAllUsers().Count == 0)
            {
                WelcomeGrid.Visibility = Visibility.Visible;
                UserName = "";
                Email = "";
                Role = 0;
            }
            else
            {
                MainNavView.SelectedItem = MainNavView.MenuItems[0];
            }
        }

        private string _name;
        private string _email;
        private Role _role;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new(propertyName));
            PropertyChanged?.Invoke(this, new(nameof(IsValidInput)));
            PropertyChanged?.Invoke(this, new(nameof(ErrorMessage)));
        }
        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }
        public bool IsValidInput
        {
            get
            {
                ErrorMessage = "";
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Email)) 
                    return false;
                bool isValid = true;
                if (string.IsNullOrEmpty(UserName))
                {
                    isValid = false;
                    ErrorMessage += "A user name is required.\n";
                }
                if (IsValidEmail(Email) == false)
                {
                    isValid = false;
                    ErrorMessage += "The email address is not valid.\n";
                }
                return isValid;
            }
        }
        public string ErrorMessage { get; set; }
        public string UserName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public Role Role
        {
            get => _role;
            set
            {
                if (value != _role)
                {
                    _role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }
        public int RoleIndex
        {
            get => (int)Role;
            set => Role = (Role)value;
        }
        /// <summary>
        /// Handle the SelectionChanged event of the MainNavView
        /// Navigate to the corresponding page when the selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MainNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // If the settings is selected, navigate to the settings page
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                // Otherwise, get the selected item. If it is not null, get its Tag
                // Navigate to Algorithm_Dynamics.Pages.<Tag>
                NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
                if (selectedItem != null)
                {
                    string tag = selectedItem.Tag as string;
                    string pageName = "Algorithm_Dynamics.Pages." + tag;
                    Type pageType = Type.GetType(pageName);
                    ContentFrame.Navigate(pageType);
                }
            }
        }

        private void OnBackButtonClick(object sender, EventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = User.Create(UserName, Email, Role);
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["CurrentUser"] = user.Uid;
            WelcomeGrid.Visibility = Visibility.Collapsed;
            MainNavView.SelectedItem = MainNavView.MenuItems[0];
        }
    }
}
