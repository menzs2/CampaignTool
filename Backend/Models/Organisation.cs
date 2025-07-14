
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("organisation")]
public partial class Organisation
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("organisation_name")]
    [StringLength(200)]
    [Required]
    public string OrganisationName { get; set; } = null!;

    [Column("description_short")]
    [StringLength(200)]
    public string DescriptionShort { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("state")]
    [StringLength(200)]
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

    [InverseProperty("OrgOne")]
    public virtual ICollection<OrgOrgConnection> OrgOrgConnectionOrgOnes { get; set; } = new List<OrgOrgConnection>();

    [InverseProperty("OrgTwo")]
    public virtual ICollection<OrgOrgConnection> OrgOrgConnectionOrgTwos { get; set; } = new List<OrgOrgConnection>();

    [NotMapped]
    public long[]? CharIds => CharOrgConnections?.Select(c => c.CharId).ToArray();}
