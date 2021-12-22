using Microsoft.UI.Xaml.Controls;
using System;

namespace Algorithm_Dynamics.Models
{
    public class HomePageGridItem
    {
        public string Name { get; set; }
        public Symbol Icon { get; set; }
        public Action Invoke { get; set; }

        public HomePageGridItem(string name)
        {
            Name = name;
        }
        public HomePageGridItem(string name, Symbol icon)
        {
            Name = name;
            Icon = icon;
        }

        public HomePageGridItem(string name, Symbol icon, Action invoke) : this(name, icon)
        {
            Invoke = invoke;
        }
    }
}
