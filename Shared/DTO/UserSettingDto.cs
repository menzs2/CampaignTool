namespace Shared;

public class UserSettingDto
{
    public long? Id { get; set; }

    public bool SelectLastCampaign { get; set; }

    public bool SameNameWarning { get; set; }

    public long? DefaultCampaignId { get; set; }
}
