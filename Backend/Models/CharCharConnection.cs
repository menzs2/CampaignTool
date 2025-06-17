using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("char_char_connection")]
public partial class CharCharConnection : BaseConnection
{
    [Column("char_one_id")]
    [Required]
    public long CharOneId { get; set; }

    [Column("char_two_id")]
    [Required]
    public long CharTwoId { get; set; }
   
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
