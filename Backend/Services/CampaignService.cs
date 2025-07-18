using Backend.Data;
using Shared;

namespace Backend.Services;

public class CampaignService
{
    public CampaignToolContext DbContext { get; }

    public CampaignService(CampaignToolContext dbContext)
    {
        DbContext = dbContext;
    }

    public IEnumerable<CampaignDto> GetAllCampaigns()
    {
        return DbContext.Campaigns.ToDto().ToList();
    }

    public CampaignDto? GetCampaignById(long id)
    {
        return DbContext.Campaigns
            .Where(c => c.Id == id).FirstOrDefault()?.ToDto();
    }

    public IEnumerable<CampaignDto> GetCampaignsByGmId(long gmId)
    {
        return DbContext.Campaigns
            .Where(c => c.Gm == gmId).ToDto().ToList();
    }

    public async Task<CampaignDto> AddCampaignAsync(CampaignDto campaignDto)
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

    public async Task<CampaignDto> UpdateCampaignAsync(CampaignDto campaignDto)
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

        existingCampaign.CampaignName = campaignDto.CampaignName;
        existingCampaign.Description = campaignDto.Description;
        existingCampaign.DescriptionShort = campaignDto.DescriptionShort;
        existingCampaign.GmOnlyDescription = campaignDto.GmOnlyDescription;
        existingCampaign.Gm = campaignDto.Gm;
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
