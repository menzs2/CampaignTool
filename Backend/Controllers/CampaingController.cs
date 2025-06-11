using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

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

    [HttpGet]
    public IActionResult Get()
    {
        var campaigns = _dbContext.Campaigns
            .Select(c => new CampaignDto
            {
                Id = c.Id,
                CampaignName = c.CampaignName,
                DescriptionShort = c.DescriptionShort,
                Description = c.Description,
                Gm = c.Gm,
                GmOnlyDescription = c.GmOnlyDescription
            })
            .ToList();

        return campaigns.Any() ? Ok(campaigns) : NotFound("No campaigns found.");
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var campaign = _dbContext.Campaigns
            .Where(c => c.Id == id)
            .Select(c => new CampaignDto
            {
                Id = c.Id,
                CampaignName = c.CampaignName,
                DescriptionShort = c.DescriptionShort,
                Description = c.Description,
                Gm = c.Gm,
                GmOnlyDescription = c.GmOnlyDescription
            })
            .FirstOrDefault();
        return campaign != null ? Ok(campaign) : NotFound($"Campaign with ID {id} not found.");
    }

    [HttpPost]
    public IActionResult Post([FromBody] Campaign campaign)
    {
        if (campaign == null)
        {
            return BadRequest("Campaign data is null.");
        }

        _dbContext.Campaigns.Add(campaign);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = campaign.Id }, campaign);
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] Campaign campaign)
    {
        if (campaign == null || campaign.Id != id)
        {
            return BadRequest("Campaign data is invalid.");
        }

        var existingCampaign = _dbContext.Campaigns.Find(id);
        if (existingCampaign == null)
        {
            return NotFound($"Campaign with ID {id} not found.");
        }

        existingCampaign.CampaignName = campaign.CampaignName;
        existingCampaign.Description = campaign.Description;
        existingCampaign.DescriptionShort = campaign.DescriptionShort;
        existingCampaign.GmOnlyDescription = campaign.GmOnlyDescription;
        existingCampaign.Gm = campaign.Gm;
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var campaign = _dbContext.Campaigns.Find(id);
        if (campaign == null)
        {
            return NotFound($"Campaign with ID {id} not found.");
        }

        _dbContext.Campaigns.Remove(campaign);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
