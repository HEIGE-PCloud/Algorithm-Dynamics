using Algorithm_Dynamics.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Algorithm_Dynamics.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [SupportedOSPlatform("windows10.0.10240.0")]
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<HomePageGridItem> Source { get; } = new ObservableCollection<HomePageGridItem>();

        public HomePage()
        {
            this.InitializeComponent();
        }
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            //MainWindow m_window = (MainWindow)((App)Application.Current).m_window;
            //if (e.ClickedItem is HomePageGridItem item)
            //{
            //    item?.Invoke();
            //}
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Source.Clear();
            Source.Add(new HomePageGridItem("Random Problem", Symbol.Home));
            Source.Add(new HomePageGridItem("Playground", Symbol.Edit));
            Source.Add(new HomePageGridItem("Assignments", Symbol.Library));
            Source.Add(new HomePageGridItem("Problems", Symbol.List));
            Source.Add(new HomePageGridItem("Settings", Symbol.Setting));
            Source.Add(new HomePageGridItem("Account", Symbol.Contact));
            Source.Add(new HomePageGridItem("Import", Symbol.Import));
            Source.Add(new HomePageGridItem("BBC Bitesize", Symbol.Link, async () => await Windows.System.Launcher.LaunchUriAsync(new Uri(@"https://www.bbc.co.uk/bitesize/subjects/z34k7ty"))));
            Source.Add(new HomePageGridItem("AQA A Level CS", Symbol.Link, async () => await Windows.System.Launcher.LaunchUriAsync(new Uri(@"https://www.aqa.org.uk/subjects/computer-science-and-it/as-and-a-level/computer-science-7516-7517"))));
            Source.Add(new HomePageGridItem("OCR A Level CS", Symbol.Link, async () => await Windows.System.Launcher.LaunchUriAsync(new Uri(@"https://www.ocr.org.uk/qualifications/as-and-a-level/computer-science-h046-h446-from-2015/"))));
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
