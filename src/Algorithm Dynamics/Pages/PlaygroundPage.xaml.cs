using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Algorithm_Dynamics.Pages
{
    public sealed partial class PlaygroundPage : Page
    {
        public PlaygroundPage()
        {
            InitializeComponent();
        }
        public string Code { get; set; } = "";

        private void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CodeEditor.Lang = LanguageComboBox.SelectedItem.ToString();
        }
    }
}
