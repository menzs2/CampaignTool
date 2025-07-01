namespace Shared;

public class BaseConnectionDto
{
    public long? Id { get; set; }
    public byte Direction { get; set; }

    public string? Description { get; set; }

     public string? GmOnlyDescription { get; set; }

    public bool GmOnly { get; set; }

     public long ConnectionId { get; set; }
}
