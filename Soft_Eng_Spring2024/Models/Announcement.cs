namespace Soft_Eng_Spring2024.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Title { get; set; }
        public string Body { get; set; }
        public int Author { get; set; }
    }
}
