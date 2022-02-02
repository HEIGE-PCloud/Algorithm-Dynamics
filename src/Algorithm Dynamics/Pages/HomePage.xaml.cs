using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class HomePage : Page
    {
        /// <summary>
        /// The WelcomeMessage is displayed to the user on the HomePage
        /// </summary>
        public string WelcomeMessage { get; set; }
        public ObservableCollection<QuickAccessItem> QAItems { get; } = new ObservableCollection<QuickAccessItem>();
        public ObservableCollection<RecommendItem> RecItems { get; } = new ObservableCollection<RecommendItem>();
        public HomePage()
        {
            SetWelcomeMessage();
            InitializeQAItems();
            InitializeRecItems();
            InitializeComponent();
        }

        /// <summary>
        /// Set the WelcomeMessage based on the current time.
        /// Morning: 00:00-12:00
        /// Afternoon: 12:00-17:00
        /// Evening 17:00-0:00
        /// TODO: Add the username after it is implemented
        /// </summary>
        private void SetWelcomeMessage()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now >= new TimeSpan(00, 00, 00) && now < new TimeSpan(12, 00, 00))
            {
                WelcomeMessage = "Good morning, user!";
            }
            else if (now >= new TimeSpan(12, 00, 00) && now < new TimeSpan(17, 00, 00))
            {
                WelcomeMessage = "Good afternoon, user!";
            }
            else if (now >= new TimeSpan(17, 00, 00) && now <= new TimeSpan(23, 59, 59))
            {
                WelcomeMessage = "Good evening, user!";
            }
        }

        /// <summary>
        /// Handle the QAGridView click event. The Action will be invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainWindow m_window = App.m_window;
            if (e.ClickedItem is QuickAccessItem item)
            {
                item.Action(m_window);
            }
        }

        /// <summary>
        /// Handle the RecGridView click event.
        /// Navigate to the corresponding Problem or Assignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // TODO: Handle the navigation.
            throw new NotImplementedException("[Blocking]: The CodingPage or the AssignmentPage has not been implemented yet.");
        }

        private void InitializeQAItems()
        {
            QAItems.Clear();
            QAItems.Add(new QuickAccessItem("Random Problem", Symbol.Shuffle, (m_window) =>
            {
                m_window.MainNavView.SelectedItem = null;
                // TODO: Navigate the ContentFrame to the Coding page manually
                throw new NotImplementedException("[Blocking]: The CodingPage has not been implemented yet.");
            }));
            QAItems.Add(new QuickAccessItem("Playground", Symbol.Edit, (m_window) =>
                m_window.MainNavView.SelectedItem = m_window.MainNavView.MenuItems[3]));
            QAItems.Add(new QuickAccessItem("Assignments", Symbol.Library, (m_window) =>
                m_window.MainNavView.SelectedItem = m_window.MainNavView.MenuItems[2]));
            QAItems.Add(new QuickAccessItem("Problems", Symbol.List, (m_window) =>
                m_window.MainNavView.SelectedItem = m_window.MainNavView.MenuItems[1]));
            QAItems.Add(new QuickAccessItem("Settings", Symbol.Setting, (m_window) =>
                m_window.MainNavView.SelectedItem = m_window.MainNavView.SettingsItem));
            QAItems.Add(new QuickAccessItem("Account", Symbol.Contact, (m_window) =>
                m_window.MainNavView.SelectedItem = m_window.MainNavView.FooterMenuItems[0]));
            QAItems.Add(new QuickAccessItem("Import", Symbol.Import, (m_window) =>
            {
                m_window.MainNavView.SelectedItem = null;
                // TODO: Handle the import logic
                throw new NotImplementedException("[Blocking]: The import logic has not been implemented yet.");
            }));
        }

        /// <summary>
        /// TODO: Generate Recommendation from database.
        /// </summary>
        private void InitializeRecItems()
        {
            RecItems.Clear();
            RecItems.Add(new RecommendItem("Problem 1", "Easy | Data structure"));
            RecItems.Add(new RecommendItem("Problem 2", "Medium | Sorting"));
            RecItems.Add(new RecommendItem("Problem 3", "Hard | Graph"));
            RecItems.Add(new RecommendItem("Problem 4", "Easy | Data structure"));
            RecItems.Add(new RecommendItem("Assignment 1", "Due in 2 days"));
            RecItems.Add(new RecommendItem("Assignment 2", "Due in 3 days"));
            RecItems.Add(new RecommendItem("Assignment 3", "Due in 4 days"));
            RecItems.Add(new RecommendItem("Assignment 4", "Due in 5 days"));
        }
    }
}
