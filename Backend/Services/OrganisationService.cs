using Backend.Data;
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
        .Where(o => connectedOrgIds.Contains(o.Id)).ToListAsync();
        return organisations?.ToDto();
    }

    public async Task<OrganisationDto?> CreateOrganisation(OrganisationDto organisationDto)
    {
        if (organisationDto == null)
        {
            throw new ArgumentNullException(nameof(OrganisationDto), "Connection DTO cannot be null");
        }
        var organisation = organisationDto.ToModel();
        if (organisation == null)
        {
            throw new InvalidOperationException("Failed to convert ConnectionDto to Connection model");
        }
        _context.Organisations.Add(organisation);
        await _context.SaveChangesAsync();
        return organisation?.ToDto();
    }

    public async Task<OrganisationDto?> UpdateOrganisation(long id, OrganisationDto organisationDto)
    {
        if (organisationDto == null)
        {
            throw new ArgumentNullException(nameof(organisationDto), "Organisation DTO cannot be null");
        }
        var existingOrganisation = await _context.Organisations.FindAsync(id);
        if (existingOrganisation == null)
        {
            throw new KeyNotFoundException($"Organisation with ID '{id}' not found.");
        }
        existingOrganisation.Description = organisationDto.Description;
        existingOrganisation.DescriptionShort = organisationDto.DescriptionShort;
        existingOrganisation.GmOnly = organisationDto.GmOnly;
        existingOrganisation.GmOnlyDescription = existingOrganisation.GmOnlyDescription;

        _context.Organisations.Update(existingOrganisation);
        await _context.SaveChangesAsync();
        return existingOrganisation?.ToDto();
    }

    public async Task DeleteOrganisation(long id)
    {
        var existingOrganisation = await _context.Organisations.FindAsync(id);
        if (existingOrganisation == null)
        {
            throw new KeyNotFoundException($"Organisation with id '{id}' not found.");
        }
        _context.Remove(existingOrganisation);
        await _context.SaveChangesAsync();
    }
}

