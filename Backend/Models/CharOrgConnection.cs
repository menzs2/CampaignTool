using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("char_organisation_connection")]
public class CharOrgConnection : BaseConnection
{
    [Column("char_id")]
    [Required]
    public long CharId { get; set; }

    [Column("organisation_id")]
    public long OrganisationId { get; set; }

    [Required]
    [Column("connection_id")]
    public long ConnectionId { get; set; }

    [ForeignKey("CharId")]
    [InverseProperty("CharOrgConnections")]
    public virtual Character Character { get; set; } = null!;

    [ForeignKey("OrganisationId")]
    [InverseProperty("CharOrgConnections")]
    public virtual Organisation Organisation { get; set; } = null!;

    [ForeignKey("ConnectionId")]
    [InverseProperty("CharOrgConnections")]
    public virtual Connection Connection { get; set; } = null!;
}   
