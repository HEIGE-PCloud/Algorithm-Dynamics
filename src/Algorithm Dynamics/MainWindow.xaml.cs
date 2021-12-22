using Microsoft.UI.Xaml;

namespace Algorithm_Dynamics
{
    /// <summary>
    /// The main window object that is used to display all other elements
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }
    }
}
