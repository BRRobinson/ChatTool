using System.ComponentModel.DataAnnotations;

namespace ChatTool.Models.DTOs;

public class UserDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }
}
