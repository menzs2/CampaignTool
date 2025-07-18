
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("organisation")]
public partial class Organisation : BaseEntity
{

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
    public long[]? CharIds => CharOrgConnections?.Select(c => c.CharId).ToArray();
}
