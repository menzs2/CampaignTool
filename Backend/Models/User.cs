
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("user")]
public partial class User
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("user_name")]
    public string UserName { get; set; } = null!;

    [Column("email")]
    public string? Email { get; set; }

    [Column("has_login")]
    public bool HasLogin { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("role")]
    public long Role { get; set; }
}
