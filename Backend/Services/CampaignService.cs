using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.Services;

public class CampaignService
{
    public CampaignToolContext DbContext { get; }

    public CampaignService(CampaignToolContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<IEnumerable<CampaignDto>> GetAllCampaigns()
    {
        var campaigns = await DbContext.Campaigns.ToListAsync();
        return campaigns.ToDto();
    }

    public async Task<CampaignDto?> GetCampaignById(long id)
    {
        var campaign = await DbContext.Campaigns.FindAsync(id);
        return campaign?.ToDto();
    }

    public async Task<IEnumerable<CampaignDto>> GetCampaignsByUserId(long userId)
    {
        var campaigns = await DbContext.Campaigns.Include(c => c.Characters)
            .Where(c => c.Gm == userId || c.Characters.Any(p => p.PlayerId == userId)).Distinct().ToListAsync();
        return campaigns.ToDto();
    }

    public async Task<CampaignDto?> AddCampaignAsync(CampaignDto? campaignDto)
    {
        if (campaignDto == null)
        {
            throw new ArgumentNullException(nameof(campaignDto), "Campaign data is null.");
        }

        var newCampaign = campaignDto.ToModel();
        if (newCampaign == null)
        {
            throw new InvalidOperationException("Failed to convert CampaignDto to Campaign model.");
        }
        DbContext.Campaigns.Add(newCampaign);
        await DbContext.SaveChangesAsync();
        return newCampaign.ToDto();
    }

    public async Task<CampaignDto?> UpdateCampaignAsync(CampaignDto? campaignDto)
    {
        if (campaignDto == null)
        {
            throw new ArgumentNullException(nameof(campaignDto), "Campaign data is null.");
        }

        var existingCampaign = await DbContext.Campaigns.FindAsync(campaignDto.Id);
        if (existingCampaign == null)
        {
            throw new KeyNotFoundException($"Campaign with ID {campaignDto.Id} not found.");
        }

        existingCampaign.Name = campaignDto.Name;
        existingCampaign.Description = campaignDto.Description;
        existingCampaign.DescriptionShort = campaignDto.DescriptionShort;
        existingCampaign.GmOnlyDescription = campaignDto.GmOnlyDescription;
        existingCampaign.Gm = campaignDto.GmId;
        DbContext.Campaigns.Update(existingCampaign);

        await DbContext.SaveChangesAsync();
        return existingCampaign.ToDto();
    }

    public async Task DeleteCampaignAsync(long id)
    {
        var campaign = await DbContext.Campaigns.FindAsync(id);
        if (campaign == null)
        {
            throw new KeyNotFoundException($"Campaign with ID {id} not found.");
        }

        DbContext.Campaigns.Remove(campaign);
        await DbContext.SaveChangesAsync();
    }
}
