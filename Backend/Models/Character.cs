using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("character")]
public partial class Character
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("character_name")]
    public string CharacterName { get; set; } = null!;

    [Column("description_short")]
    public string DescriptionShort { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("state")]
    public string? State { get; set; }

    [Column("campaign_id")]
    public long? CampaignId { get; set; }

    [Column("player_id")]
    public long? PlayerId { get; set; }

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [Column("gm_only")]
    public bool? GmOnly { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Characters")]
    public virtual Campaign? Campaign { get; set; }

    [InverseProperty("CharOne")]
    public virtual ICollection<CharCharConnection> CharCharConnectionCharOnes { get; set; } = new List<CharCharConnection>();

    [InverseProperty("CharTwo")]
    public virtual ICollection<CharCharConnection> CharCharConnectionCharTwos { get; set; } = new List<CharCharConnection>();

    [NotMapped]
    public long[] CharCharConnectionIds => CharCharConnectionCharOnes.Select(c => c.Id).Concat(CharCharConnectionCharTwos.Select(c => c.Id)).Distinct().ToArray();
}
