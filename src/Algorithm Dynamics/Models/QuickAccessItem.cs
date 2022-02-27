using Microsoft.UI.Xaml.Controls;
using System;

namespace Algorithm_Dynamics.Models
{
    public class QuickAccessItem
    {
        public string Name { get; set; }
        public Symbol Icon { get; set; }
        public Action Action { get; set; }
        public QuickAccessItem(string name, Symbol icon, Action action)
        {
            Name = name;
            Icon = icon;
            Action = action;
        }
    }
}
