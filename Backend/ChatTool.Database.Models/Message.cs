using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
