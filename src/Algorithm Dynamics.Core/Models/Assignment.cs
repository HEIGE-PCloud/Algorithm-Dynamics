using System;

namespace Algorithm_Dynamics.Core.Models
{
    public class Assignment : ProblemList
    {
        public Guid Uid { get; set; }
        public DateTime DueDate { get; set; }
        public AssignmentStatus Status { get; set; }
        public AssignmentType Type { get; set; }
        public User Assigner { get; set; }
        public Assignment(string name, string description, DateTime dueDate)
        {
            Uid = Guid.NewGuid();
            Name = name;
            Description = description;
            DueDate = dueDate;
        }
    }
    public enum AssignmentStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OverDue
    }
    public enum AssignmentType
    {
        Source,
        Copy
    }
}
