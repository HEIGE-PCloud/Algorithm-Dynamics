using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class ProblemsPage : Page
    {
        public ProblemsPage()
        {
            InitializeComponent();
        }
        private readonly ObservableCollection<string> Difficulties = new() { "Easy", "Medium", "Hard" };
        private readonly ObservableCollection<string> Statuses = new() { "Todo", "Attempted", "Done" };
        public ObservableCollection<string> Lists = new() { "List 1", "List 2", "List 3" };
        public ObservableCollection<string> Tags = new() { "Tag 1", "Tag 2", "Tag 3"};

        private void ListComboBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ListMenuFlyout.ShowAt(comboBox, e.GetPosition(comboBox));
        }

        /// <summary>
        /// Clear the ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearComboBox(object sender, RightTappedRoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.SelectedIndex = -1;
        }
        /// <summary>
        /// Clear the ListComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearComboBox(object sender, RoutedEventArgs e)
        {
            ListComboBox.SelectedIndex = -1;
        }
    }
}
