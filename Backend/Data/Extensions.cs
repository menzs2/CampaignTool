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
        var newCampaign = new Campaign
        {
            CampaignName = campaignDto.CampaignName,
            DescriptionShort = campaignDto.DescriptionShort,
            Description = campaignDto.Description,
            Gm = campaignDto.Gm,
            GmOnlyDescription = campaignDto.GmOnlyDescription
        };
        if (campaignDto.Id.HasValue)
        {
            newCampaign.Id = campaignDto.Id.Value;
        }
        return newCampaign;

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
        var newCharacter = new Character
        {
            CharacterName = characterDto.CharacterName,
            Description = characterDto.Description,
            CampaignId = characterDto.CampaignId,
            PlayerId = characterDto.PlayerId,
            DescriptionShort = characterDto.DescriptionShort,
        };
        if (characterDto.Id.HasValue)
        {
            newCharacter.Id = characterDto.Id.Value;
        }
        return newCharacter;
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

    #region Organisation Extensions

    public static OrganisationDto? ToDto(this Organisation? organisation)
    {
        if (organisation == null) return null;

        return new OrganisationDto
        {
            Id = organisation.Id,
            OrganisationName = organisation.OrganisationName,
            DescriptionShort = organisation.DescriptionShort,
            Description = organisation.Description,
            State = organisation.State,
            CampaignId = organisation.CampaignId,
            GmOnly = organisation.GmOnly,
            GmOnlyDescription = organisation.GmOnlyDescription
        };
    }

    public static List<OrganisationDto> ToDto(this IEnumerable<Organisation?> organisations)
    {
        var organisationsDtoList = new List<OrganisationDto>();
        foreach (var organisation in organisations)
        {
            if (organisation.ToDto() is OrganisationDto dto)
            {
                organisationsDtoList.Add(dto);
            }
        }
        return organisationsDtoList;
    }

    public static Organisation? ToModel(this OrganisationDto? organisationDto)
    {
        if (organisationDto == null) return null;
       var newOrganisation = new Organisation
        {
            OrganisationName = organisationDto.OrganisationName,
            DescriptionShort = organisationDto.DescriptionShort,
            Description = organisationDto.Description,
            State = organisationDto.State,
            CampaignId = organisationDto.CampaignId,
            GmOnly = organisationDto.GmOnly,
            GmOnlyDescription = organisationDto.GmOnlyDescription
        };
        if (organisationDto.Id.HasValue)
        {
            newOrganisation.Id = organisationDto.Id.Value;
        }
        return newOrganisation;
    }

    public static List<Organisation> ToModel(this IEnumerable<OrganisationDto?> organisationDtos)
    {
        var organisations = new List<Organisation>();
        foreach (var organisationDto in organisationDtos)
        {
            if (organisationDto.ToModel() is Organisation organisation)
            {
                organisations.Add(organisation);
            }
        }
        return organisations;
    }

    #endregion

    #region User Extensions

    public static UserDto? ToDto(this User? user)
    {
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            HasLogin = user.HasLogin,
            Password = user.Password,
            Role = user.Role
        };
    }

    public static List<UserDto> ToDto(this IEnumerable<User?> users)
    {
        var usersDtoList = new List<UserDto>();
        foreach (var user in users)
        {
            if (user.ToDto() is UserDto dto)
            {
                usersDtoList.Add(dto);
            }
        }
        return usersDtoList;
    }

    public static User? ToModel(this UserDto? userDto)
    {
        if (userDto == null) return null;
        var newUser = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            UserName = userDto.UserName,
            Email = userDto.Email,
            HasLogin = userDto.HasLogin,
            Password = userDto.Password,
            Role = userDto.Role
        };
        if (userDto.Id.HasValue)
        {
            newUser.Id = userDto.Id.Value;
        }
        return newUser;
    }

    public static List<User> ToModel(this IEnumerable<UserDto?> userDtos)
    {
        var users = new List<User>();
        foreach (var userDto in userDtos)
        {
            if (userDto.ToModel() is User user)
            {
                users.Add(user);
            }
        }
        return users;
    }
    #endregion

    #region UserSetting Extensions

    public static UserSettingDto? ToDto(this UserSetting? userSetting)
    {
        if (userSetting == null) return null;

        return new UserSettingDto
        {
            Id = userSetting.Id,
            SelectLastCampaign = userSetting.SelectLastCampaign,
            SameNameWarning = userSetting.SameNameWarning,
            DefaultCampaignId = userSetting.DefaultCampaignId
        };
    }

    public static List<UserSettingDto> ToDto(this IEnumerable<UserSetting?> userSettings)
    {
        var userSettingsDtoList = new List<UserSettingDto>();
        foreach (var userSetting in userSettings)
        {
            if (userSetting.ToDto() is UserSettingDto dto)
            {
                userSettingsDtoList.Add(dto);
            }
        }
        return userSettingsDtoList;
    }

    public static UserSetting? ToModel(this UserSettingDto? userSettingDto)
    {
        if (userSettingDto == null) return null;
        var userSetting = new UserSetting{
            SelectLastCampaign = userSettingDto.SelectLastCampaign,
            SameNameWarning = userSettingDto.SameNameWarning,
            DefaultCampaignId = userSettingDto.DefaultCampaignId
        };
        if (userSettingDto.Id.HasValue)
        {
            userSetting.Id = userSettingDto.Id.Value;
        }
        return userSetting;
    }

    public static List<UserSetting> ToModel(this IEnumerable<UserSettingDto?> userSettingDtos)
    {
        var userSettings = new List<UserSetting>();
        foreach (var userSettingDto in userSettingDtos)
        {
            if (userSettingDto.ToModel() is UserSetting userSetting)
            {
                userSettings.Add(userSetting);
            }
        }
        return userSettings;
    }

    #endregion
}
