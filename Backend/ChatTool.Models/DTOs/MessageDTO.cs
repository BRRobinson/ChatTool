using System.ComponentModel.DataAnnotations;

namespace ChatTool.Models.DTOs;

public class MessageDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required ChatDTO Chat { get; set; }

    [Required]
    public required UserDTO Sender { get; set; }

    public DateTime SentAt { get; set; }

    [Required]
    public required string message { get; set; }
}
