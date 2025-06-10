using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("campaign")]
public partial class Campaign
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("campaign_name")]
    public string CampaignName { get; set; } = null!;

    [Column("description_short")]
    public string DescriptionShort { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("gm")]
    public long Gm { get; set; }

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [InverseProperty("Campaign")]
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    [InverseProperty("Campaign")]
    public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();

    [InverseProperty("Campaign")]
    public virtual ICollection<Organisation> Organisations { get; set; } = new List<Organisation>();

    [InverseProperty("DefaultCampaign")]
    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
}
