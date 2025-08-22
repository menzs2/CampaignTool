namespace Shared.DTO
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }

        public UserDto? User { get; set; }

        public string? Message { get; set; }
    }
}