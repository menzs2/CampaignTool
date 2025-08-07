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
    public async Task<IActionResult> Get()
    {
        var campaigns = await _campaignService.GetAllCampaigns();
        return campaigns.Any() ? Ok(campaigns) : NotFound("No campaigns found.");
    }

    /// <summary>
    /// Gets a campaign by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var campaign = await _campaignService.GetCampaignById(id);
        return campaign != null ? Ok(campaign) : Problem(detail: $"Campaign with ID {id} not found.", statusCode: 404);
    }

    /// <summary>
    /// Gets campaigns associated with a specific GM by their ID.
    /// </summary>
    [HttpGet("gm/{gmId}")]
    public async Task<IActionResult> GetByGmId(long gmId)
    {
        var campaigns = await _campaignService.GetCampaignsByGmId(gmId);
        return campaigns.Any() ? Ok(campaigns) : Problem($"No campaigns found for GM with ID {gmId}.", statusCode: 404);
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
        catch (Exception ex)
        {
            // TODO: Log the exception.
            return Problem("Internal server error",statusCode: 500);
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
        catch (KeyNotFoundException)
        {
            return BadRequest($"Campaign with key '{id}' not found.");
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            // TODO: Log the exception.
            return StatusCode(500, "Internal server error");
        }
    }
}
