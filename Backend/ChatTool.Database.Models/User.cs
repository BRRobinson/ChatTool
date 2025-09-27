using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatTool.Database.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }

        [JsonIgnore]
        public List<Chat> Chats { get; set; } = new List<Chat>();

        [JsonIgnore]
        public List<Message> messages { get; set; } = new List<Message>();
    }
}
