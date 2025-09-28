using System.ComponentModel.DataAnnotations;

namespace ChatTool.Models.DTOs;

public class ChatDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required List<UserDTO> Participants { get; set; } = new List<UserDTO>();
}
