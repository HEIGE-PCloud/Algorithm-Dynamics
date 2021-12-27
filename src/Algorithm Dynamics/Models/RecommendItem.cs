namespace Algorithm_Dynamics.Models
{
    public class RecommendItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public RecommendItem(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
