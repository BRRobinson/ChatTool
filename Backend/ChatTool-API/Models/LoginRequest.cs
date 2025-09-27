using System.ComponentModel.DataAnnotations;

namespace ChatTool.API.Models
{
    public class LoginRequest
    {
        [Required]
        public required string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}