using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.AuthResultDto
{
    public class UserRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}