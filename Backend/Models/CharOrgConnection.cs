using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("char_organisation_connection")]
public class CharOrgConnection
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("char_id")]
    [Required]
    public long CharId { get; set; }

    [Column("organisation_id")]
    public long OrganisationId { get; set; }

    [Column("direction")]
    public byte[] Direction { get; set; } = null!;

    [Column("description")]
    [StringLength(500)]
    public string? Description { get; set; }

    [Column("gm_only_description")]
    [StringLength(1000)]
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
