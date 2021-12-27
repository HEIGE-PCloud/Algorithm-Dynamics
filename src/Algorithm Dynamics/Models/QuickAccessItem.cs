using Microsoft.UI.Xaml.Controls;

namespace Algorithm_Dynamics.Models
{
    public class QuickAccessItem
    {
        public string Name { get; set; }
        public Symbol Icon { get; set; }
        public QuickAccessItem(string name, Symbol icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}
