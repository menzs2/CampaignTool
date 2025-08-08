using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("campaign")]
public partial class Campaign : BaseEntity
{

    [ForeignKey("User")]
    [Column("gm")]
    public long Gm { get; set; }
    
    [InverseProperty("Campaign")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Campaign")]
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    [InverseProperty("Campaign")]
    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();

    [InverseProperty("Campaign")]
    public virtual ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();

    [InverseProperty("DefaultCampaign")]
    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
