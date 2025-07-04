﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("user_setting")]
public partial class UserSetting
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("select_last_campaign")]
    public bool SelectLastCampaign { get; set; }

    [Column("same_name_warning")]
    public bool SameNameWarning { get; set; }

    [Column("default_campaign_id")]
    public long? DefaultCampaignId { get; set; }

    [ForeignKey("DefaultCampaignId")]
    [InverseProperty("UserSettings")]
    public virtual Campaign? DefaultCampaign { get; set; }

    [Column("user_id")]
    [Required]
    public long UserId { get; set; }

    [ForeignKey("UserId")]
    [Required]
    [InverseProperty("UserSetting")]
    public virtual User User { get; set; } = null!;
}
