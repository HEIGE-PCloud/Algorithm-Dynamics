using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class HomePageGridItem
    {
        public string Name { get; set; }
        public Symbol Icon { get; set; }

        public HomePageGridItem(string name)
        {
            Name = name;
        }
        public HomePageGridItem(string name, Symbol icon)
        {
            Name = name;
            Icon = icon;
        }
    }
}
