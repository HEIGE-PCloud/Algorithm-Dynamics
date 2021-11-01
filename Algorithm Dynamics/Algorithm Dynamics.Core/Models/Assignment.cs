using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Dynamics.Core.Models
{
    public class Assignment : ProblemList
    {
        public new Guid Id { get; set; }
        public new string Name { get; set; }
        public new string Description { get; set; }
        public AssignmentStatus Status { get; set; }
        public AssignmentType Type { get; set; }
        public DateTime DueDate { get; set; }
        public Assignment(string name, string description, DateTime dueDate, ProblemList problemList)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueDate = dueDate;
            AddRange(problemList);
        }
        public enum AssignmentStatus
        {
            NotStarted,
            InProgress,
            Completed,
            Overdue
        }
        public enum AssignmentType
        {
            Source,
            Copy
        }
    }
}
