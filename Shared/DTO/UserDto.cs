namespace Shared;

public class UserDto
{
public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public bool HasLogin { get; set; }

    public string? Password { get; set; }

    public long Role { get; set; }
}
