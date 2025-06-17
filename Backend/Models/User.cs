using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("user")]
public partial class User
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Column("last_name")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Column("user_name")]
    [StringLength(30)]
    public string UserName { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    [Column("has_login")]
    public bool HasLogin { get; set; }

    [Column("password")]
    public string? Password { get; set; }

    [Column("role")]
    public long Role { get; set; }

    [InverseProperty("User")]
    public virtual UserSetting UserSetting { get; set; } = null!;
}
