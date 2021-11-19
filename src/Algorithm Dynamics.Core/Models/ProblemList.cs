using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Models
{
    public class ProblemList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Problem> Problems { get; set; }
        public int Count { get { return Problems.Count; } }
        public void Add(Problem problem)
        {
            Problems.Add(problem);
        }
    }
}
