using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("character")]
public partial class Character : BaseEntity
{

    [Column("player_id")]
    public long? PlayerId { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Characters")]
    public virtual Campaign? Campaign { get; set; }

    [InverseProperty("CharOne")]
    public virtual ICollection<CharCharConnection> CharCharConnectionCharOnes { get; set; } = new List<CharCharConnection>();

    [InverseProperty("CharTwo")]
    public virtual ICollection<CharCharConnection> CharCharConnectionCharTwos { get; set; } = new List<CharCharConnection>();

    [InverseProperty("Character")]
    public virtual ICollection<CharOrgConnection> CharOrgConnections { get; set; } = new List<CharOrgConnection>();

    [NotMapped]
    public long[] CharCharConnectionIds => CharCharConnectionCharOnes.Select(c => c.Id).Concat(CharCharConnectionCharTwos.Select(c => c.Id)).Distinct().ToArray();
}
