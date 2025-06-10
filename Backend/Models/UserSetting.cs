using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("user_setting")]
public partial class UserSetting
{
    [Key]
    [Column("id")]
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
}
