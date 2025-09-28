using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatTool.Database.Models;

public class Chat
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required List<User> Participants { get; set; } = new List<User>();

    [JsonIgnore]
    public List<Message> Messages { get; set; } = new List<Message>();
}
