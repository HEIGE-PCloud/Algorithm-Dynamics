using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Assignment : ProblemList
    {
        public new string Name { get; set; }
        public new string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
