using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampaignController : ControllerBase
{
    private readonly CampaignService _campaignService;

    public CampaignController(CampaignService campaignService)
    {
        _campaignService = campaignService;
    }

    /// <summary>
    /// Gets all campaigns.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var campaigns = _campaignService.GetAllCampaigns();
            return campaigns.Any() ? Ok(campaigns) : NotFound("No campaigns found.");
        }
        catch
        {
            // Log the exception (not implemented here)
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets a campaign by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var campaign = _campaignService.GetCampaignById(id);
        return campaign != null ? Ok(campaign) : NotFound($"Campaign with ID {id} not found.");
    }

    /// <summary>
    /// Gets campaigns associated with a specific GM by their ID.
    /// </summary>
    [HttpGet("gm/{gmId}")]
    public IActionResult GetByGmId(long gmId)
    {
        var campaigns = _campaignService.GetCampaignsByGmId(gmId);
        return campaigns.Any() ? Ok(campaigns) : NotFound($"No campaigns found for GM with ID {gmId}.");
    }

    /// <summary>
    /// Adds a new campaign.
    /// </summary>
    /// <param name="campaign">The campaign data to add.</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CampaignDto campaign)
    {
        try
        {
            var newCampaign = await _campaignService.AddCampaignAsync(campaign);
            return CreatedAtAction(nameof(Get), new { id = newCampaign.Id }, newCampaign);
        }
        catch
        {
            // TODO: Log the exception.
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to update.</param>
    /// <param name="campaign">The updated campaign data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] CampaignDto campaign)
    {
        try
        {
            var updatedCampaign = await _campaignService.UpdateCampaignAsync(campaign);
            return NoContent();
        }
        catch
        {
            // TODO: Log the exception.
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deletes a campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _campaignService.DeleteCampaignAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Campaign with ID {id} not found.");
        }
        catch
        {
            // TODO: Log the exception.
            return StatusCode(500, "Internal server error");
        }
    }
}
