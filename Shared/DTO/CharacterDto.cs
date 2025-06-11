namespace Shared;

public class CharacterDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public long CampaignId { get; set; }
    public long PlayerId { get; set; }
    public string? GmOnlyDescription { get; set; }
}
