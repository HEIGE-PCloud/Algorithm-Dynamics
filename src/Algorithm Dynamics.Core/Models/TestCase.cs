using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class TestCase
    {
        public int Id { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsExample { get; set; }
    }
}
