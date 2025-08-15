using System.ComponentModel.DataAnnotations;

namespace Shared.DTO
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
