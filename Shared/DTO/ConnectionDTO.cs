namespace Shared;

public class ConnectionDTO
{
    public long Id { get; set; }

    public string ConnectionName { get; set; } = null!;

    public string? Description { get; set; }

    public string? GmOnlyDescription { get; set; }
    
    public bool GmOnly { get; set; }

    public long? CampaignId { get; set; }

}
