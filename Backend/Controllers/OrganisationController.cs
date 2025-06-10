using Backend.Data;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Organisations.Any()
            ? Ok(_dbContext.Organisations.ToList())
            : NotFound("No organisations found.");
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var organisation = _dbContext.Organisations.Find(id);
        return organisation != null ? Ok(organisation) : NotFound($"Organisation with ID {id} not found.");
    }

    [HttpGet("campaign/{campaignId}")]
    public IActionResult GetByCampaign(long campaignId)
    {
        var organisations = _dbContext.Organisations.Where(o => o.CampaignId == campaignId).ToList();
        return organisations.Any() ? Ok(organisations) : NotFound($"No organisations found for campaign ID {campaignId}.");
    }

    [HttpGet("connected/{id}")]
    public IActionResult GetConnectedOrganisations(long id)
    {
        var organisations = _dbContext.Organisations
            .Where(o => o.Id != id && o.CharOrgConnections.Any(c => c.OrganisationId == id))
            .ToList();
        return organisations.Any() ? Ok(organisations) : NotFound("No connected organisations found.");
    }

    [HttpPost]
    public IActionResult Post([FromBody] Models.Organisation organisation)
    {
        if (organisation == null)
        {
            return BadRequest("Organisation data is null.");
        }

        _dbContext.Organisations.Add(organisation);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = organisation.Id }, organisation);
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] Models.Organisation organisation)
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

        existingOrganisation.OrganisationName = organisation.OrganisationName;
        existingOrganisation.Description = organisation.Description;
        existingOrganisation.GmOnlyDescription = organisation.GmOnlyDescription;
        existingOrganisation.GmOnly = organisation.GmOnly;

        _dbContext.SaveChanges();
        return NoContent();
    }

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
