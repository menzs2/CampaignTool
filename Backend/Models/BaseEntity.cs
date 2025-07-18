using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend;

public abstract class BaseEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("name")]
    [StringLength(200)]
    [Required]
    public string Name { get; set; } = null!;

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

    [Column("gm_only_description")]
    public string? GmOnlyDescription { get; set; }

    [Column("gm_only")]
    public bool? GmOnly { get; set; }
}
