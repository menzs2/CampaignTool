using Backend.Models;

namespace Shared;

public static class Extensions
{
    public static CampaignDto? ToDto(this Campaign campaign)
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

    public static List<CampaignDto> ToDto(this IEnumerable<Campaign> campaigns)
    {
        var campaignsDtoList = new List<CampaignDto>();
        foreach (var campaign in campaigns)
        {
            if (campaign.ToDto() is CampaignDto dto)
            {
                campaignsDtoList.Add(dto);
            }
        }
        return campaignsDtoList;
    }

    public static Campaign? ToModel(this CampaignDto campaignDto)
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
        var campaigns = new List<Campaign>();
        foreach (var campaignDto in campaignDtos)
        {
            if (campaignDto.ToModel() is Campaign campaign)
            {
                campaigns.Add(campaign);
            }
        }
        return campaigns;
    }
}
