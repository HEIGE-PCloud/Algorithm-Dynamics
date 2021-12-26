using Microsoft.UI.Xaml.Controls;

namespace Algorithm_Dynamics.Models
{
    public class HomePageGridViewItem
    {
        public string Name { get; set; }
        public Symbol Icon { get; set; }
        public HomePageGridViewItem(string name, Symbol icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}
