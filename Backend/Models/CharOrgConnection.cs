using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("char_organisation_connection")]
public class CharOrgConnection
{
[Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("char_id")]
    public long CharId { get; set; }

    [Column("organisation_id")]
    public long OrganisationId { get; set; }

    [Column("direction")]
    public byte[] Direction { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [Column("gm_only")]
    public bool GmOnly { get; set; }

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
