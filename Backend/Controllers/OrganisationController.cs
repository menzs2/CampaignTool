using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/[controller]")]
[ApiController]
public class OrganisationController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public OrganisationController(CampaignToolContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// gets all organisations.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Organisations.Any()
            ? Ok(_dbContext.Organisations.ToDto())
            : NotFound("No organisations found.");
    }

    /// <summary>
    /// gets an organisation by its ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to retrieve.</param>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var organisation = _dbContext.Organisations.Where(o => o.Id == id).FirstOrDefault().ToDto();
        return organisation != null ? Ok(organisation) : NotFound($"Organisation with ID {id} not found.");
    }

    /// <summary>
    /// Retrieves organisations associated with a specific campaign.
    /// </summary>
    /// <param name="campaignId">The ID of the campaign to retrieve organisations for.</param>
    [HttpGet("campaign/{campaignId}")]
    public IActionResult GetByCampaign(long campaignId)
    {
        var organisations = _dbContext.Organisations.Where(o => o.CampaignId == campaignId).ToDto();
        return organisations.Any() ? Ok(organisations) : NotFound($"No organisations found for campaign ID {campaignId}.");
    }

    /// <summary>
    /// Retrieves organisations connected to a specific organisation by ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to find connections for.</param>
    [HttpGet("connected/{id}")]
    public IActionResult GetConnectedOrganisations(long id)
    {
        var organisations = _dbContext.Organisations
            .Where(o => o.Id != id && o.CharOrgConnections.Any(c => c.OrganisationId == id))
            .ToDto();
        return organisations.Any() ? Ok(organisations) : NotFound("No connected organisations found.");
    }

    /// <summary>
    /// Creates a new organisation.
    /// </summary>
    /// <param name="organisation">The organisation data to create.</param>
    [HttpPost]
    public IActionResult Post([FromBody] OrganisationDto? organisation)
    {
        if (organisation == null)
        {
            return BadRequest("Organisation data is null.");
        }
        var newOrganisation = organisation.ToModel();
        if (newOrganisation == null)
        {
            return BadRequest("Invalid organisation data.");
        }
        var organisationId = _dbContext.Organisations.Add(newOrganisation).Entity.Id;
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = organisation.Id }, organisation);
    }

    /// <summary>
    /// Updates an existing organisation.
    /// </summary>
    /// <param name="id">The ID of the organisation to update.</param>
    /// <param name="organisation">The updated organisation data.</param>
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] OrganisationDto? organisation)
    {
        if (organisation == null || organisation.Id != id)
        {
            return BadRequest("Organisation data is invalid.");
        }

        var existingOrganisation = _dbContext.Organisations.Find(id);
        if (existingOrganisation == null)
        {
            return NotFound($"Organisation with ID {id} not found.");
        }

        existingOrganisation.Name = organisation.Name;
        existingOrganisation.Description = organisation.Description;
        existingOrganisation.GmOnlyDescription = organisation.GmOnlyDescription;
        existingOrganisation.GmOnly = organisation.GmOnly;

        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes an organisation by its ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to delete.</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var organisation = _dbContext.Organisations.Find(id);
        if (organisation == null)
        {
            return NotFound($"Organisation with ID {id} not found.");
        }

        _dbContext.Organisations.Remove(organisation);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
