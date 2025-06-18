using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CampaignController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;
    // Constructor to inject DbContext if needed
    public CampaignController(CampaignToolContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// Gets all campaigns.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        var campaigns = _dbContext.Campaigns.ToDto().ToList();
        return campaigns.Any() ? Ok(campaigns) : NotFound("No campaigns found.");
    }

    /// <summary>
    /// Gets a campaign by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var campaign = _dbContext.Campaigns
            .Where(c => c.Id == id).FirstOrDefault()?.ToDto();
        return campaign != null ? Ok(campaign) : NotFound($"Campaign with ID {id} not found.");
    }

    /// <summary>
    /// Gets campaigns associated with a specific GM by their ID.
    /// </summary>
    [HttpGet("gm/{gmId}")]
    public IActionResult GetByGmId(long gmId)
    {
        var campaigns = _dbContext.Campaigns
            .Where(c => c.Gm == gmId).ToDto().ToList();
        return campaigns.Any() ? Ok(campaigns) : NotFound($"No campaigns found for GM with ID {gmId}.");
    }

    /// <summary>
    /// Adds a new campaign.
    /// </summary>
    /// <param name="campaign">The campaign data to add.</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CampaignDto campaign)
    {
        if (campaign == null)
        {
            return BadRequest("Campaign data is null.");
        }
        var newCampaign = campaign.ToModel();
        if (newCampaign == null)
        {
            return BadRequest("Invalid campaign data.");
        }
        _dbContext.Campaigns.Add(newCampaign);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = newCampaign.Id }, campaign);
    }
    /// <summary>
    /// Updates an existing campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to update.</param>
    /// <param name="campaign">The updated campaign data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] CampaignDto campaign)
    {
        if (campaign == null || campaign.Id != id)
        {
            return BadRequest("Campaign data is invalid.");
        }

        var existingCampaign = await _dbContext.Campaigns.FindAsync(id);
        if (existingCampaign == null)
        {
            return NotFound($"Campaign with ID {id} not found.");
        }

        existingCampaign.CampaignName = campaign.CampaignName;
        existingCampaign.Description = campaign.Description;
        existingCampaign.DescriptionShort = campaign.DescriptionShort;
        existingCampaign.GmOnlyDescription = campaign.GmOnlyDescription;
        existingCampaign.Gm = campaign.Gm;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a campaign by its ID.
    /// </summary>
    /// <param name="id">The ID of the campaign to delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var campaign = await _dbContext.Campaigns.FindAsync(id);
        if (campaign == null)
        {
            return NotFound($"Campaign with ID {id} not found.");
        }

        _dbContext.Campaigns.Remove(campaign);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
