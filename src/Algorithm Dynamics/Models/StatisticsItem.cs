using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Models
{
    public class StatisticsItem
    {
        public StatisticsItem(string title, string data)
        {
            Title = title;
            Data = data;
        }

        public string Title { get; set; }
        public string Data { get; set; }
    }
}
