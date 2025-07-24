using Backend.Data;

namespace Backend;

public class OrganisationService
{
    private readonly CampaignToolContext _context;

    public OrganisationService(CampaignToolContext context)
    {
        _context = context;
    }
}
