using System;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Core.Models
{
    public class Assignment : ProblemList
    {
        internal Assignment(int id, string name, string description, List<Problem> problems) : base(id, name, description, problems)
        {
        }

        public Guid Uid { get; set; }
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; }
        public AssignmentType Type { get; set; }
        public User Assigner { get; set; }
        //public Assignment(string name, string description, DateTime dueDate)
        //{
        //    Uid = Guid.NewGuid();
        //    Name = name;
        //    Description = description;
        //    DueDate = dueDate;
        //}
    }
    public enum AssignmentStatus
    {
        Draft,
        Scheduled,
        Published,
        Assigned,
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
