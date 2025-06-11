namespace Shared;

public class OrganisationDto
{
public long Id { get; set; }

    public string OrganisationName { get; set; } = null!;

    public string DescriptionShort { get; set; } = null!;

    public string? Description { get; set; }

    public string? State { get; set; }

    public long? CampaignId { get; set; }

    public bool GmOnly { get; set; }

    public string? GmOnlyDescription { get; set; }

}
