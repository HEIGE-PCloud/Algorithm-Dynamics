using System;

namespace Algorithm_Dynamics.Models
{
    public class RecommendItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }
        public RecommendItem(string title, string description, Action action)
        {
            Title = title;
            Description = description;
            Action = action;
        }
    }
}
