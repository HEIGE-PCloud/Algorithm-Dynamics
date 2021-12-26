using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class HomePage : Page
    {
        public string WelcomeMessage { get; set; }
        public ObservableCollection<HomePageGridViewItem> Items { get; } = new ObservableCollection<HomePageGridViewItem>();
        public HomePage()
        {
            SetWelcomeMessage();
            Items.Clear();
            Items.Add(new HomePageGridViewItem("Random Problem", Symbol.Shuffle));
            Items.Add(new HomePageGridViewItem("Playground", Symbol.Edit));
            Items.Add(new HomePageGridViewItem("Assignments", Symbol.Library));
            Items.Add(new HomePageGridViewItem("Problems", Symbol.List));
            Items.Add(new HomePageGridViewItem("Settings", Symbol.Setting));
            Items.Add(new HomePageGridViewItem("Account", Symbol.Contact));
            Items.Add(new HomePageGridViewItem("Import", Symbol.Import));
            InitializeComponent();
            // ProblemRecPanel.Children.Insert(0, new HyperlinkButton());
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
                WelcomeMessage = "Good morning, User!";
            }
            else if (now >= new TimeSpan(12, 00, 00) && now < new TimeSpan(17, 00, 00))
            {
                WelcomeMessage = "Good afternoon, User!";
            }
            else if (now >= new TimeSpan(17, 00, 00) && now <= new TimeSpan(23, 59, 59))
            {
                WelcomeMessage = "Good evening, User!";
            }
        }
    }
}
