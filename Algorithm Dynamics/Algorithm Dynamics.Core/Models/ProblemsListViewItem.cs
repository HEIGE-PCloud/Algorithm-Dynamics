using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class ProblemsListViewItem
    {
        public string ProblemName { get; set; }
        public ProblemsListViewItem(string problemName)
        {
            ProblemName = problemName;
        }
    }
}
