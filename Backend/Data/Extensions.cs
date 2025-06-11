using Backend.Models;

namespace Shared;

public static class Extensions
{
    #region Campaign Extensions

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

    #endregion

    #region Character Extensions

    public static CharacterDto? ToDto(this Character? character)
    {
        if (character == null) return null;

        return new CharacterDto
        {
            Id = character.Id,
            CharacterName = character.CharacterName,
            Description = character.Description,
            CampaignId = character.CampaignId,
            PlayerId = character.PlayerId,
            DescriptionShort = character.DescriptionShort,
        };
    }

    public static List<CharacterDto> ToDto(this IEnumerable<Character?> characters)
    {
        var charactersDtoList = new List<CharacterDto>();
        foreach (var character in characters)
        {
            if (character.ToDto() is CharacterDto dto)
            {
                charactersDtoList.Add(dto);
            }
        }
        return charactersDtoList;
    }

    public static Character? ToModel(this CharacterDto? characterDto)
    {
        if (characterDto == null) return null;

        return new Character
        {
            Id = characterDto.Id,
            CharacterName = characterDto.CharacterName,
            Description = characterDto.Description,
            CampaignId = characterDto.CampaignId,
            PlayerId = characterDto.PlayerId,
            DescriptionShort = characterDto.DescriptionShort,
        };
    }

    public static List<Character> ToModel(this IEnumerable<CharacterDto?> characterDtos)
    {
        var characters = new List<Character>();
        foreach (var characterDto in characterDtos)
        {
            if (characterDto.ToModel() is Character character)
            {
                characters.Add(character);
            }
        }
        return characters;
    }

    #endregion
}
