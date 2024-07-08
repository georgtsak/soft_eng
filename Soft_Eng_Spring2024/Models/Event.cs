using System.ComponentModel.DataAnnotations;

namespace Soft_Eng_Spring2024.Models
{
    public class Event
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Author { get; set; }
    }
}
