using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using

namespace Backend.Models;

public abstract class BaseConnection
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("direction")]
    [Required]
    public byte[] Direction { get; set; } = null!;

    [Column("description")]
    [StringLength(500)]
    [Required]
    public string? Description { get; set; }

    [Column("gm_only_description")]
    [StringLength(1000)]
    public string? GmOnlyDescription { get; set; }

    [Column("gm_only")]
    public bool GmOnly { get; set; }

}
