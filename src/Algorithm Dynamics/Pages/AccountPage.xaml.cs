using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        }
        public ObservableCollection<StatisticsItem> StatsItems { get; } = new ObservableCollection<StatisticsItem>();

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
    }

}
