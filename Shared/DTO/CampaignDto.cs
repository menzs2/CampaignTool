namespace Shared;

public class CampaignDto
{
    public long Id { get; set; }
    public string CampaignName { get; set; } = null!;
    public string DescriptionShort { get; set; } = null!;
    public string? Description { get; set; }
    public long Gm { get; set; }
    public string? GmOnlyDescription { get; set; }
}
