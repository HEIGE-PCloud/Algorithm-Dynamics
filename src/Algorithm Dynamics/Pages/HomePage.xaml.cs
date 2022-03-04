using Algorithm_Dynamics.Core.Helpers;
using Algorithm_Dynamics.Core.Models;
using Algorithm_Dynamics.Helpers;
using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;

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
            if (e.ClickedItem is QuickAccessItem item)
            {
                item.Action();
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
            QAItems.Add(new QuickAccessItem("Random Problem", Symbol.Shuffle, () =>
            {
                // Get all problems from the database
                List<Problem> problems= DataAccess.GetAllProblems();

                // Generate a random index
                var random = new Random();
                int randomIndex = random.Next(problems.Count);

                // Get the corresponding problem
                Problem problem = problems[randomIndex];

                // Navigate to coding page with the problems as parameters
                App.NavigateTo(typeof(CodingPage), Tuple.Create(problem, problems));
            }));
            QAItems.Add(new QuickAccessItem("Playground", Symbol.Edit, () => App.NavigateTo(typeof(PlaygroundPage))));
            QAItems.Add(new QuickAccessItem("Assignments", Symbol.Library, () => App.NavigateTo(typeof(AssignmentsPage))));
            QAItems.Add(new QuickAccessItem("Problems", Symbol.List, () => App.NavigateTo(typeof(ProblemsPage))));
            QAItems.Add(new QuickAccessItem("Settings", Symbol.Setting, () => App.NavigateTo(typeof(SettingsPage))));
            QAItems.Add(new QuickAccessItem("Account", Symbol.Contact, () => App.NavigateTo(typeof(AccountPage))));
            QAItems.Add(new QuickAccessItem("Import", Symbol.Import, async () =>
            {
                StorageFile file = await FileHelper.FileOpenPicker("*");
                // TODO: Import into the database
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
