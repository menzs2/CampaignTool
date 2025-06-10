using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("char_char_connection")]
public partial class CharCharConnection
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("char_one_id")]
    public long CharOneId { get; set; }

    [Column("char_two_id")]
    public long CharTwoId { get; set; }

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

    [ForeignKey("CharOneId")]
    [InverseProperty("CharCharConnectionCharOnes")]
    public virtual Character CharOne { get; set; } = null!;

    [ForeignKey("CharTwoId")]
    [InverseProperty("CharCharConnectionCharTwos")]
    public virtual Character CharTwo { get; set; } = null!;

    [ForeignKey("ConnectionId")]
    [InverseProperty("CharCharConnections")]
    public virtual Connection Connection { get; set; } = null!;
}
