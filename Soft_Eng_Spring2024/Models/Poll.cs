namespace Soft_Eng_Spring2024.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public int[] Votes { get; set; }
        
    }
}
