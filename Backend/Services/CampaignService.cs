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

}
