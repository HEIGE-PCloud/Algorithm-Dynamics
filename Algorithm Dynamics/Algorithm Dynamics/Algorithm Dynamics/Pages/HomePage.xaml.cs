using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<HomePageGridItem> Source { get; } = new ObservableCollection<HomePageGridItem>();

        public HomePage()
        {
            this.InitializeComponent();
        }
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is HomePageGridItem item)
            {
                MainWindow mainWindow = (MainWindow)((App)Application.Current).m_window;
                switch (item.Name)
                {
                    case "Problems":
                        mainWindow.SetSelectedNavigationItem(1);
                        break;
                    case "Home":
                    default:
                        mainWindow.SetSelectedNavigationItem(0);
                        break;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Source.Clear();
            Source.Add(new HomePageGridItem("Home", Symbol.Home));
            Source.Add(new HomePageGridItem("Problems", Symbol.List));
            Source.Add(new HomePageGridItem("Import Problems", Symbol.Import));
            Source.Add(new HomePageGridItem("Import Problem Lists", Symbol.Import));
            Source.Add(new HomePageGridItem("Import Assignments", Symbol.Import));
            Source.Add(new HomePageGridItem("Pick a Random Problem", Symbol.Shuffle));
            Source.Add(new HomePageGridItem("Playground", Symbol.Edit));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
