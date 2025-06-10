
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("connection")]
public partial class Connection
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("connection_name")]
    public string ConnectionName { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [Column("gm_only")]
    public bool GmOnly { get; set; }

    [Column("campaign_id")]
    public long? CampaignId { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Connections")]
    public virtual Campaign? Campaign { get; set; }

    [InverseProperty("Connection")]
    public virtual ICollection<CharCharConnection> CharCharConnections { get; set; } = new List<CharCharConnection>();
}
