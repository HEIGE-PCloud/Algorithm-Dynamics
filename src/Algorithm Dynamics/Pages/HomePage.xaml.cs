using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class HomePage : Page
    {
        public string WelcomeMessage { get; set; }
        public ObservableCollection<QuickAccessItem> QAItems { get; } = new ObservableCollection<QuickAccessItem>();
        public ObservableCollection<RecommendItem> RecItems { get; } = new ObservableCollection<RecommendItem>();
        public HomePage()
        {
            SetWelcomeMessage();
            QAItems.Clear();
            QAItems.Add(new QuickAccessItem("Random Problem", Symbol.Shuffle));
            QAItems.Add(new QuickAccessItem("Playground", Symbol.Edit));
            QAItems.Add(new QuickAccessItem("Assignments", Symbol.Library));
            QAItems.Add(new QuickAccessItem("Problems", Symbol.List));
            QAItems.Add(new QuickAccessItem("Settings", Symbol.Setting));
            QAItems.Add(new QuickAccessItem("Account", Symbol.Contact));
            QAItems.Add(new QuickAccessItem("Import", Symbol.Import));
            RecItems.Clear();
            RecItems.Add(new RecommendItem("Problem 1", "Easy | Data structure"));
            RecItems.Add(new RecommendItem("Problem 2", "Medium | Sorting"));
            RecItems.Add(new RecommendItem("Problem 3", "Hard | Graph"));
            RecItems.Add(new RecommendItem("Problem 4", "Easy | Data structure"));
            RecItems.Add(new RecommendItem("Assignment 1", "Due in 2 days"));
            RecItems.Add(new RecommendItem("Assignment 2", "Due in 3 days"));
            RecItems.Add(new RecommendItem("Assignment 3", "Due in 4 days"));
            RecItems.Add(new RecommendItem("Assignment 4", "Due in 5 days"));
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
                WelcomeMessage = "Good morning, PCloud!";
            }
            else if (now >= new TimeSpan(12, 00, 00) && now < new TimeSpan(17, 00, 00))
            {
                WelcomeMessage = "Good afternoon, PCloud!";
            }
            else if (now >= new TimeSpan(17, 00, 00) && now <= new TimeSpan(23, 59, 59))
            {
                WelcomeMessage = "Good evening, PCloud!";
            }
        }
    }
}
