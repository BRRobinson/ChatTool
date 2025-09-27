using System.ComponentModel.DataAnnotations;

namespace ChatTool.Database.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required Chat Chat { get; set; }

        [Required]
        public required User Sender { get; set; }

        public DateTime SentAt { get; set; }

        [Required]
        public required string message { get; set; }
    }
}
