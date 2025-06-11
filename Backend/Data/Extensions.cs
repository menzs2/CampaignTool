using Backend.Models;

namespace Shared;

public static class Extensions
{
    public static CampaignDto ToDto(this Campaign campaign)
    {
        if (campaign == null) return null;

        return new CampaignDto
        {
            Id = campaign.Id,
            CampaignName = campaign.CampaignName,
            DescriptionShort = campaign.DescriptionShort,
            Description = campaign.Description,
            Gm = campaign.Gm,
            GmOnlyDescription = campaign.GmOnlyDescription
        };
    }

    public static IEnumerable<CampaignDto> ToDto(this IEnumerable<Campaign> campaigns)
    {
        return campaigns?.Select(c => c.ToDto()) ?? Enumerable.Empty<CampaignDto>();
    }

    public static Campaign ToModel(this CampaignDto campaignDto)
    {
        if (campaignDto == null) return null;

        return new Campaign
        {
            Id = campaignDto.Id,
            CampaignName = campaignDto.CampaignName,
            DescriptionShort = campaignDto.DescriptionShort,
            Description = campaignDto.Description,
            Gm = campaignDto.Gm,
            GmOnlyDescription = campaignDto.GmOnlyDescription
        };
    }

    public static IEnumerable<Campaign> ToModel(this IEnumerable<CampaignDto> campaignDtos)
    {
        return campaignDtos?.Select(c => c.ToModel()) ?? Enumerable.Empty<Campaign>();
    }
}
