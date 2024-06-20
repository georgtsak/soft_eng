using System.ComponentModel.DataAnnotations;

namespace Soft_Eng_Spring2024.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly FinishDate { get; set; }

        public string Votes { get; set; } = ""; 
        
    }
}
