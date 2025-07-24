using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend;

public class OrganisationService
{
    private readonly CampaignToolContext _context;

    public OrganisationService(CampaignToolContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrganisationDto>?> GetOrganisationDtoAsync()
    {
        var organisations = await _context.Organisations.ToListAsync();
        return organisations?.ToDto();
    }
    
    public async Task<IEnumerable<OrganisationDto>?> GetOrganisationDtoAsync(long campaignId)
    {
        var organisations = await _context.Organisations.Where(o => o.CampaignId == campaignId).ToListAsync();
        return organisations?.ToDto();
    }

    public async Task<OrganisationDto?> GetOrganisationDtoByIdAsync(long id)
    {
        var organisation = await _context.Organisations.FindAsync(id);
        return organisation?.ToDto();
    }

    public async Task<IEnumerable<OrganisationDto>?> GetOrganisationsByNameAsync(string name)
    {
        var organisations = await _context.Organisations.Where(o => o.Name == name).ToListAsync();
        return organisations?.ToDto();
    }

    public async Task<IEnumerable<OrganisationDto>?> GetConnectedOrganisationsAsync(long organisationId)
    {
        var connections = await _context.OrgOrgConnections
        .Where(oo => oo.OrgOneId == organisationId || oo.OrgTwoId == organisationId)
        .ToListAsync();

        var connectedOrgIds = connections
        .SelectMany(oi => new[] { oi.OrgOneId, oi.OrgTwoId })
        .Where(oi => oi != organisationId).Distinct();

        var organisations = await _context.Organisations
        .Where(o => connectedOrgIds.Contains(organisationId)).ToListAsync();
        return organisations?.ToDto();
    }

}

