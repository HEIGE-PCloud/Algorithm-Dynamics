using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class AccountPage : Page, INotifyPropertyChanged
    {
        public AccountPage()
        {
            InitializeComponent();
            StatsItems.Add(new StatisticsItem("Problem Solved", "10"));
            StatsItems.Add(new StatisticsItem("Problem Attempted", "3"));
            StatsItems.Add(new StatisticsItem("Problem Unsolved", "1100"));
            StatsItems.Add(new StatisticsItem("Correct Rate", "10%"));
            StatsItems.Add(new StatisticsItem("Favourite Topic", "Data structure"));
            StatsItems.Add(new StatisticsItem("Favourite Language", "Python"));
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            var CurrentUserValue = roamingSettings.Values["CurrentUser"];
            if (CurrentUserValue != null)
            {
                Guid CurrentUserUid = (Guid)CurrentUserValue;
                _user = User.Get(CurrentUserUid);
            }
            else
            {
                _user = User.Create("PCloud", "heige.pcloud@outlook.com", Role.Student);
                roamingSettings.Values["CurrentUser"] = _user.Uid;
            }


        }
        private ObservableCollection<StatisticsItem> StatsItems { get; } = new ObservableCollection<StatisticsItem>();

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(IsNotEditMode));
            }
        }
        public bool IsNotEditMode
        {
            get => !_isEditMode;
        }
        public int RoleIndex
        {
            get => (int)_user.Role;
            set => _user.Role = (Role)value;
        }

        /// <summary>
        /// Invoke when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Invoke a new <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Use <see cref="nameof"/> to get the name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditMode == false)
            {
                IsEditMode = true;
                EditButton.Content = "Done";
            }
            else
            {
                IsEditMode = false;
                EditButton.Content = "Edit";
            }
        }

        private User _user;
    }

}
