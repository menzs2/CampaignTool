using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/[controller]")]
[ApiController]
public class OrganisationController : ControllerBase
{
    //private readonly CampaignToolContext _dbContext;
    private readonly OrganisationService _service;

    public OrganisationController(OrganisationService service)
    {
        _service = service;
    }

    /// <summary>
    /// gets all organisations.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var organisations = await _service.GetOrganisationDtoAsync();
        return organisations != null && organisations.Any()
            ? Ok(organisations)
            : NotFound("No organisations found.");
    }

    /// <summary>
    /// gets an organisation by its ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to retrieve.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var organisation = await _service.GetOrganisationDtoByIdAsync(id);
        return organisation != null ? Ok(organisation) : NotFound($"Organisation with ID {id} not found.");
    }

    /// <summary>
    /// Retrieves organisations associated with a specific campaign.
    /// </summary>
    /// <param name="campaignId">The ID of the campaign to retrieve organisations for.</param>
    [HttpGet("campaign/{campaignId}")]
    public async Task<IActionResult> GetByCampaign(long campaignId)
    {
        var organisations = await _service.GetOrganisationDtoAsync(campaignId);
        return organisations != null && organisations.Any() ? Ok(organisations) : NotFound($"No organisations found for campaign ID {campaignId}.");
    }

    /// <summary>
    /// Retrieves organisations connected to a specific organisation by ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to find connections for.</param>
    [HttpGet("connected/{id}")]
    public async Task<IActionResult> GetConnectedOrganisations(long id)
    {
        var organisations = await _service.GetConnectedOrganisationsAsync(id);
        return organisations != null && organisations.Any() ? Ok(organisations) : NotFound("No connected organisations found.");
    }

    /// <summary>
    /// Creates a new organisation.
    /// </summary>
    /// <param name="organisation">The organisation data to create.</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrganisationDto? organisation)
    {
        if (organisation == null)
        {
            return BadRequest("Organisation data is null.");
        }
        try
        {
            await _service.CreateOrganisation(organisation);
            return CreatedAtAction(nameof(Get), new { id = organisation.Id }, organisation);

        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing organisation.
    /// </summary>
    /// <param name="id">The ID of the organisation to update.</param>
    /// <param name="organisation">The updated organisation data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] OrganisationDto? organisation)
    {
        if (organisation == null || organisation.Id != id)
        {
            return BadRequest("Organisation data is invalid.");
        }

        try
        {
            await _service.UpdateOrganisation(id, organisation);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes an organisation by its ID.
    /// </summary>
    /// <param name="id">The ID of the organisation to delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _service.DeleteOrganisation(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }
}
