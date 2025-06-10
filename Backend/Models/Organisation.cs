
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("organisation")]
public partial class Organisation
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("organisation_name")]
    public string OrganisationName { get; set; } = null!;

    [Column("description_short")]
    public string DescriptionShort { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("state")]
    public string? State { get; set; }

    [Column("campaign_id")]
    public long? CampaignId { get; set; }

    [Column("gm_only")]
    public bool GmOnly { get; set; }

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [ForeignKey("CampaignId")]
    [InverseProperty("Organisations")]
    public virtual Campaign? Campaign { get; set; }
    
    [InverseProperty("Organisation")]
    public virtual ICollection<CharOrgConnection> CharOrgConnections { get; set; } = new List<CharOrgConnection>();
}
