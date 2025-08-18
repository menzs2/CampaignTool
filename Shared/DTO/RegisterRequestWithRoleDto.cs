using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public class RegisterRequestWithRoleDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Role { get; set; } // Optional role for registration

        public UserDto? User { get; set; }
    }
}
