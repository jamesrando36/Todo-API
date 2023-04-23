using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.UserRequestDtos
{
    public class UserLoginRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
