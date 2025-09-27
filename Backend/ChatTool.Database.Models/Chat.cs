using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatTool.Database.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; }
        
        public required List<User> Participants { get; set; } = new List<User>();

        [JsonIgnore]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
