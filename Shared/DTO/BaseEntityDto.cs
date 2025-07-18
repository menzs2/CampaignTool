namespace Shared;

public class BaseEntityDto
{
    public long? Id { get; set; }

    public string Name { get; set; } = null!;

    public string DescriptionShort { get; set; } = null!;

    public string? Description { get; set; }

    public string? State { get; set; }

    public long? CampaignId { get; set; }

    public string? GmOnlyDescription { get; set; }

    public bool? GmOnly { get; set; }
}
