﻿using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class AccountPage : Page, INotifyPropertyChanged
    {
        public AccountPage()
        {
            InitializeComponent();
            StatsItems.Add(new("Problem Solved", Problem.All.Count(problem => problem.Status == ProblemStatus.Solved).ToString()));
            StatsItems.Add(new("Problem Attempted", Problem.All.Count(problem => problem.Status == ProblemStatus.Attempted).ToString()));
            StatsItems.Add(new("Problem Todo", Problem.All.Count(problem => problem.Status == ProblemStatus.Todo).ToString()));
            if (Submission.All.Count > 0)
                StatsItems.Add(new("Correct Rate", $"{(SubmissionResult.All.Count(result => result.ResultCode == ResultCode.SUCCESS) * 100 / Submission.All.Count)}%"));
            else
                StatsItems.Add(new("Correct Rate", "0%"));
            string favTag = "";
            int maxTag = 0;
            foreach (var tag in Core.Models.Tag.All)
            {
                int tagCnt = Submission.All.Count(submission => submission.Problem.Tags.Contains(tag));
                if (tagCnt > maxTag)
                {
                    maxTag = tagCnt;
                    favTag = tag.Name;
                }
            }
            StatsItems.Add(new("Favourite Topic", favTag));
            string favLang = "";
            int maxLang = 0;
            foreach (var lang in Core.Models.Language.All)
            {
                int langCnt = Submission.All.Count(submission => Equals(submission.Language, lang));
                if (langCnt > maxLang)
                {
                    maxLang = langCnt;
                    favLang = lang.DisplayName;
                }
            }
            StatsItems.Add(new("Favourite Language", favLang));
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            var CurrentUserValue = localSettings.Values["CurrentUser"];
            if (CurrentUserValue != null)
            {
                Guid CurrentUserUid = (Guid)CurrentUserValue;
                _user = User.Get(CurrentUserUid);
            }
            else
            {
                _user = User.Create("PCloud", "heige.pcloud@outlook.com", Role.Student);
                localSettings.Values["CurrentUser"] = _user.Uid;
            }
            UserName = _user.Name;
            Email = _user.Email;
            Role = _user.Role;
        }
        private ObservableCollection<StatisticsItem> StatsItems { get; } = new();
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
            get => (int)Role;
            set => Role = (Role)value;
        }

        /// <summary>
        /// Invoke when the property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke a new <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Use <see cref="nameof"/> to get the name of the property.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new(propertyName));
            PropertyChanged?.Invoke(this, new(nameof(IsValidInput)));
            PropertyChanged?.Invoke(this, new(nameof(ErrorMessage)));
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
                _user.Name = UserName;
                _user.Email = Email;
                _user.Role = Role;
            }
        }

        private User _user;
        private string _name;
        private string _email;
        private Role _role;
        public string UserName
        {
            get => _name;
            set
            {
                if (value != _name)
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

        public bool IsValidInput
        {
            get
            {
                ErrorMessage = "";
                if (IsEditMode == false)
                    return true;
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
        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

    }
}
