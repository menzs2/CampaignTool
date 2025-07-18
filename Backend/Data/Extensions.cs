using Backend.Models;
using Shared;

namespace Backend.Data;

public static class Extensions
{
    #region Campaign Extensions

    public static CampaignDto? ToDto(this Campaign? campaign)
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

    public static IEnumerable<CampaignDto> ToDto(this IEnumerable<Campaign?> campaigns)
        => campaigns.Where(c => c != null).Select(c => c!.ToDto()!).ToList();

    public static Campaign? ToModel(this CampaignDto? campaignDto)
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
            newCampaign.Id = campaignDto.Id.Value;
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
            Name = character.Name,
            Description = character.Description,
            CampaignId = character.CampaignId,
            PlayerId = character.PlayerId,
            DescriptionShort = character.DescriptionShort,
            GmOnly = character.GmOnly,
            GmOnlyDescription = character.GmOnlyDescription
        };
    }

    public static IEnumerable<CharacterDto> ToDto(this IEnumerable<Character?> characters)
    {
        return characters
            .Where(c => c != null)
            .Select(c => c!.ToDto()!)
            .ToList();
    }

    public static Character? ToModel(this CharacterDto? characterDto)
    {
        if (characterDto == null) return null;
        var newCharacter = new Character
        {
            Name = characterDto.Name,
            Description = characterDto.Description,
            CampaignId = characterDto.CampaignId,
            PlayerId = characterDto.PlayerId,
            DescriptionShort = characterDto.DescriptionShort,
            GmOnly = characterDto.GmOnly,
            GmOnlyDescription = characterDto.GmOnlyDescription
        };
        if (characterDto.Id.HasValue)
        {
            newCharacter.Id = characterDto.Id.Value;
        }
        return newCharacter;
    }

    public static IEnumerable<Character> ToModel(this IEnumerable<CharacterDto?> characterDtos)
    {
        return characterDtos
            .Where(c => c != null)
            .Select(c => c!.ToModel()!)
            .ToList();
    }

    #endregion

    #region Organisation Extensions

    public static OrganisationDto? ToDto(this Organisation? organisation)
    {
        if (organisation == null) return null;

        return new OrganisationDto
        {
            Id = organisation.Id,
            Name = organisation.Name,
            DescriptionShort = organisation.DescriptionShort,
            Description = organisation.Description,
            State = organisation.State,
            CampaignId = organisation.CampaignId,
            GmOnly = organisation.GmOnly,
            GmOnlyDescription = organisation.GmOnlyDescription
        };
    }

    public static IEnumerable<OrganisationDto> ToDto(this IEnumerable<Organisation?> organisations)
    {
        return organisations
            .Where(o => o != null)
            .Select(o => o!.ToDto()!)
            .ToList();
    }

    public static Organisation? ToModel(this OrganisationDto? organisationDto)
    {
        if (organisationDto == null) return null;
        var newOrganisation = new Organisation
        {
            Name = organisationDto.Name,
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

    public static IEnumerable<Organisation> ToModel(this IEnumerable<OrganisationDto?> organisationDtos)
    {
        return organisationDtos
            .Where(o => o != null)
            .Select(o => o!.ToModel()!)
            .ToList();
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

    public static IEnumerable<UserDto> ToDto(this IEnumerable<User?> users)
    {
        return users
            .Where(u => u != null)
            .Select(u => u!.ToDto()!)
            .ToList();
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

    public static IEnumerable<User> ToModel(this IEnumerable<UserDto?> userDtos)
    {
        return userDtos
            .Where(u => u != null)
            .Select(u => u!.ToModel()!)
            .ToList();
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

    public static IEnumerable<UserSettingDto> ToDto(this IEnumerable<UserSetting?> userSettings)
    {
        return userSettings
            .Where(u => u != null)
            .Select(u => u!.ToDto()!)
            .ToList();
    }

    public static UserSetting? ToModel(this UserSettingDto? userSettingDto)
    {
        if (userSettingDto == null) return null;
        var userSetting = new UserSetting
        {
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

    public static IEnumerable<UserSetting> ToModel(this IEnumerable<UserSettingDto?> userSettingDtos)
    {
        return userSettingDtos
            .Where(u => u != null)
            .Select(u => u!.ToModel()!)
            .ToList();
    }

    #endregion

    #region Connection Extensions
    public static ConnectionDto? ToDto(this Connection? connection)
    {
        if (connection == null) return null;

        return new ConnectionDto
        {
            Id = connection.Id,
            ConnectionName = connection.ConnectionName,
            Description = connection.Description,
            GmOnlyDescription = connection.GmOnlyDescription,
            GmOnly = connection.GmOnly,
            CampaignId = connection.CampaignId
        };
    }

    public static IEnumerable<ConnectionDto> ToDto(this IEnumerable<Connection?> connections)
    {
        return connections
            .Where(c => c != null)
            .Select(c => c!.ToDto()!)
            .ToList();
    }
    public static Connection? ToModel(this ConnectionDto? connectionDto)
    {
        if (connectionDto == null) return null;
        var newConnection = new Connection
        {
            ConnectionName = connectionDto.ConnectionName,
            Description = connectionDto.Description,
            GmOnlyDescription = connectionDto.GmOnlyDescription,
            GmOnly = connectionDto.GmOnly,
            CampaignId = connectionDto.CampaignId
        };
        if (connectionDto.Id.HasValue)
        {
            newConnection.Id = connectionDto.Id.Value;
        }
        return newConnection;
    }

    public static CharCharConnectionDto? ToDto(this CharCharConnection charCharConnection)
    {
        if (charCharConnection == null) return null;

        return new CharCharConnectionDto
        {
            Id = charCharConnection.Id,
            Direction = charCharConnection.Direction,
            Description = charCharConnection.Description,
            GmOnlyDescription = charCharConnection.GmOnlyDescription,
            GmOnly = charCharConnection.GmOnly,
            CharOneId = charCharConnection.CharOneId,
            CharTwoId = charCharConnection.CharTwoId,
            ConnectionId = charCharConnection.ConnectionId,
        };
    }
    public static IEnumerable<CharCharConnectionDto> ToDto(this IEnumerable<CharCharConnection> charCharConnections)
    {
        return charCharConnections
            .Where(c => c != null)
            .Select(c => c!.ToDto()!)
            .ToList();
    }

    public static CharCharConnection? ToModel(this CharCharConnectionDto? charCharConnectionDto)
    {
        if (charCharConnectionDto == null) return null;
        var newCharCharConnection = new CharCharConnection
        {
            Direction = charCharConnectionDto.Direction,
            Description = charCharConnectionDto.Description,
            GmOnlyDescription = charCharConnectionDto.GmOnlyDescription,
            GmOnly = charCharConnectionDto.GmOnly,
            CharOneId = charCharConnectionDto.CharOneId,
            CharTwoId = charCharConnectionDto.CharTwoId,
            ConnectionId = charCharConnectionDto.ConnectionId
        };
        if (charCharConnectionDto.Id.HasValue)
        {
            newCharCharConnection.Id = charCharConnectionDto.Id.Value;
        }
        return newCharCharConnection;
    }

    public static IEnumerable<CharCharConnection> ToModel(this IEnumerable<CharCharConnectionDto?> charCharConnectionDtos)
    {
        return charCharConnectionDtos
            .Where(c => c != null)
            .Select(c => c!.ToModel()!)
            .ToList();
    }

    public static CharOrgConnectionDto? ToDto(this CharOrgConnection charOrgConnection)
    {
        if (charOrgConnection == null) return null;

        return new CharOrgConnectionDto
        {
            Id = charOrgConnection.Id,
            Direction = charOrgConnection.Direction,
            Description = charOrgConnection.Description,
            GmOnlyDescription = charOrgConnection.GmOnlyDescription,
            GmOnly = charOrgConnection.GmOnly,
            CharId = charOrgConnection.CharId,
            OrganisationId = charOrgConnection.OrganisationId
        };
    }

    public static IEnumerable<CharOrgConnectionDto> ToDto(this IEnumerable<CharOrgConnection> charOrgConnections)
    {
        return charOrgConnections
            .Where(c => c != null)
            .Select(c => c!.ToDto()!)
            .ToList();
    }

    public static CharOrgConnection? ToModel(this CharOrgConnectionDto? charOrgConnectionDto)
    {
        if (charOrgConnectionDto == null) return null;
        var newCharOrgConnection = new CharOrgConnection
        {
            Direction = charOrgConnectionDto.Direction,
            Description = charOrgConnectionDto.Description,
            GmOnlyDescription = charOrgConnectionDto.GmOnlyDescription,
            GmOnly = charOrgConnectionDto.GmOnly,
            CharId = charOrgConnectionDto.CharId,
            OrganisationId = charOrgConnectionDto.OrganisationId
        };
        if (charOrgConnectionDto.Id.HasValue)
        {
            newCharOrgConnection.Id = charOrgConnectionDto.Id.Value;
        }
        return newCharOrgConnection;
    }

    public static IEnumerable<CharOrgConnection> ToModel(this IEnumerable<CharOrgConnectionDto?> charOrgConnectionDtos)
    {
        return charOrgConnectionDtos
            .Where(c => c != null)
            .Select(c => c!.ToModel()!)
            .ToList();
    }

    public static OrgOrgConnectionDto? ToDto(this OrgOrgConnection orgOrgConnection)
    {
        if (orgOrgConnection == null) return null;

        return new OrgOrgConnectionDto
        {
            Id = orgOrgConnection.Id,
            Direction = orgOrgConnection.Direction,
            Description = orgOrgConnection.Description,
            GmOnlyDescription = orgOrgConnection.GmOnlyDescription,
            GmOnly = orgOrgConnection.GmOnly,
            OrgOneId = orgOrgConnection.OrgOneId,
            OrgTwoId = orgOrgConnection.OrgTwoId
        };
    }

    public static IEnumerable<OrgOrgConnectionDto> ToDto(this IEnumerable<OrgOrgConnection> orgOrgConnections)
    {
        return orgOrgConnections
            .Where(c => c != null)
            .Select(c => c!.ToDto()!)
            .ToList();
    }

    public static OrgOrgConnection? ToModel(this OrgOrgConnectionDto? orgOrgConnectionDto)
    {
        if (orgOrgConnectionDto == null) return null;
        var newOrgOrgConnection = new OrgOrgConnection
        {
            Direction = orgOrgConnectionDto.Direction,
            Description = orgOrgConnectionDto.Description,
            GmOnlyDescription = orgOrgConnectionDto.GmOnlyDescription,
            GmOnly = orgOrgConnectionDto.GmOnly,
            OrgOneId = orgOrgConnectionDto.OrgOneId,
            OrgTwoId = orgOrgConnectionDto.OrgTwoId
        };
        if (orgOrgConnectionDto.Id.HasValue)
        {
            newOrgOrgConnection.Id = orgOrgConnectionDto.Id.Value;
        }
        return newOrgOrgConnection;
    }

    public static IEnumerable<OrgOrgConnection> ToModel(this IEnumerable<OrgOrgConnectionDto?> orgOrgConnectionDtos)
    {
        return orgOrgConnectionDtos
            .Where(c => c != null)
            .Select(c => c!.ToModel()!)
            .ToList();
    }

    #endregion
}
