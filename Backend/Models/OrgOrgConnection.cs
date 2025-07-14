using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("organisation_organisation_connection")]
public class OrgOrgConnection : BaseConnection
{
    [Column("org_one_id")]
    [Required]
    public long OrgOneId { get; set; }

    [Column("org_two_id")]
    [Required]
    public long OrgTwoId { get; set; }

    [Column("connection_id")]
    public long ConnectionId { get; set; }

    [ForeignKey("OrgOneId")]
    [InverseProperty("OrgOrgConnectionOrgOnes")]
    public virtual Organisation OrgOne { get; set; } = null!;

    [ForeignKey("OrgTwoId")]
    [InverseProperty("OrgOrgConnectionOrgTwos")]
    public virtual Organisation OrgTwo { get; set; } = null!;

    [ForeignKey("ConnectionId")]
    [InverseProperty("OrgOrgConnections")]
    public virtual Connection Connection { get; set; } = null!;


}
