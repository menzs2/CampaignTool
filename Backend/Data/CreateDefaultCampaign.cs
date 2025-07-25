using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class CreateDefaultCampaign
{
    private readonly CampaignToolContext _dbContext;

    public CreateDefaultCampaign(CampaignToolContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Execute()
    {
        _dbContext.Database.EnsureCreated();
        //truncate the database tables if they exist
        _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE campaign, connection, \"user\", character, char_char_connection, char_organisation_connection RESTART IDENTITY CASCADE");
        _dbContext.SaveChanges();
        // Create default users, campaigns, and connections if they do not exist
        CreateDefaultUsers();
        CreateDefaultCampaigns();
        CreateDefaultConnections();
        CreateDefaultPlayerCharacters();
        CreateDefaultNPCs();
    }

    private void CreateDefaultUsers()
    {
        if (!_dbContext.Users.Any())
        {
            var userList = new List<User>()
            {
                new User
                {
                    FirstName = "Default", LastName = "User", UserName = "defaultuser", Email = "defaultuser@example.com", HasLogin = true, Password = "defaultpassword", Role = 0
                },
                new User
                {
                    FirstName = "Default", LastName = "User One", UserName = "defaultuserone", Email = "defaultuserone@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                },
                                new User
                {
                    FirstName = "Default", LastName = "User Two", UserName = "defaultusertwo", Email = "defaultusertwo@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                },
                new User
                {
                    FirstName = "Default", LastName = "User Three", UserName = "defaultuserthree", Email = "defaultuserthree@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                },
                new User
                {
                    FirstName = "Default", LastName = "User Four", UserName = "defaultuserfour", Email = "defaultuserfour@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                },
                new User
                {
                    FirstName = "Default", LastName = "User Five", UserName = "defaultuserfive", Email = "defaultuserfive@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                },
                new User
                {
                    FirstName = "Default", LastName = "User Six", UserName = "defaultusersix", Email = "defaultusersix@example.com", HasLogin = true, Password = "defaultpassword", Role = 1
                }
            };

            _dbContext.Users.AddRange(userList);
            _dbContext.SaveChanges();
            foreach (var item in _dbContext.Users)
            {
                item.UserSetting = new UserSetting
                {
                    UserId = item.Id,
                    SameNameWarning = true,
                    SelectLastCampaign = false,
                    DefaultCampaignId = null // No default campaign initially
                };
            }
        }
    }

    private void CreateDefaultCampaigns()
    {
        if (!_dbContext.Campaigns.Any())
        {
            var campaigns = new List<Campaign>
            {
                new Campaign
                {
                    CampaignName = "Default Campaign",
                    Description = "This is a default campaign.",
                    Gm = 1, // Assuming the first user is the GM
                    DescriptionShort = "Default campaign short description.",
                }
            };

            _dbContext.Campaigns.AddRange(campaigns);
            _dbContext.SaveChanges();
        }
    }
    private void CreateDefaultConnections()
    {
        if (!_dbContext.Connections.Any())
        {
            var connections = new List<Connection>
            {
                new Connection
                {
                    ConnectionName = "Friend",
                    Description = "The Characters are good friends.",
                    GmOnlyDescription = "GM only info for default connection.",
                    GmOnly = false,
                    CampaignId = 1 // Assuming the first campaign is the default one
                },
                new Connection{
                    ConnectionName = "Enemy",
                    Description = "The Characters are enemies.",
                    GmOnlyDescription = "GM only info for enemy connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Ally",
                    Description = "The Characters are allies.",
                    GmOnlyDescription = "GM only info for ally connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Rival",
                    Description = "The Characters are rivals.",
                    GmOnlyDescription = "GM only info for rival connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Mentor",
                    Description = "The Characters have a mentor relationship.",
                    GmOnlyDescription = "GM only info for mentor connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Family",
                    Description = "The Characters are family members.",
                    GmOnlyDescription = "GM only info for family connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Stranger",
                    Description = "The Characters are strangers.",
                    GmOnlyDescription = "GM only info for stranger connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Superior",
                    Description = "The Characters have a superior-subordinate relationship.",
                    GmOnlyDescription = "GM only info for superior connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Business Partner",
                    Description = "The Characters are business partners.",
                    GmOnlyDescription = "GM only info for business partner connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Rival Organization",
                    Description = "The Characters are part of rival organizations.",
                    GmOnlyDescription = "GM only info for rival organization connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Member",
                    Description = "The Characters is part of the organization.",
                    GmOnlyDescription = "GM only info for member of the same organization connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                },
                new Connection
                {
                    ConnectionName = "Love Interest",
                    Description = "The Characters have a romantic relationship.",
                    GmOnlyDescription = "GM only info for love interest connection.",
                    GmOnly = false,
                    CampaignId = 1, // Assuming the first campaign is the default one
                }
            };

            _dbContext.Connections.AddRange(connections);
            _dbContext.SaveChanges();
        }
    }

    private void CreateDefaultPlayerCharacters()
    {
        if (!_dbContext.Characters.Any())
        {
            var characters = new List<Character>
            {
                new Character
                {
                    Name = "Lord Willowdale",
                    DescriptionShort = "Minor noble with a penchant for occultism.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 2 // Assuming the second user is the owner
                },
                new Character
                {
                    Name = "Michael Jaeger",
                    DescriptionShort = "A Witcher in training.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 3 // Assuming the third user is the owner
                },
                new Character
                {
                    Name = "Anne Holmes",
                    DescriptionShort = "A young women with mysterious powers. Twin sister of Antonia.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 4 // Assuming the fourth user is the owner
                },
                new Character
                {
                    Name = "Antonia \"Toni\" Holmes",
                    DescriptionShort = "A young women, dressing as a paper boy. Twin sister of Anne.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 5 // Assuming the fifth user is the owner
                },
                new Character
                {
                    Name = "Sister Agnes",
                    DescriptionShort = "A stern nun from Silesia.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 6 // Assuming the sixth user is the owner
                },
            };

            _dbContext.Characters.AddRange(characters);
            _dbContext.SaveChanges();
        }
    }
    private void CreateDefaultNPCs()
    {
        if (!_dbContext.Characters.Any(c => c.PlayerId == 1))
        {
            var npcs = new List<Character>
            {
                new Character
                {
                    Name = "Howard",
                    DescriptionShort = "the coachman of Lord Willowdale.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Mary",
                    DescriptionShort = "Foster mother of Anne and Antonia.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Elydion",
                    DescriptionShort = "A demon and father of Anne and Antonia.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Mary",
                    DescriptionShort = "Foster Mother of Anne and Antonia. Owner of a pub in London.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Father Sheamus",
                    DescriptionShort = "A priest in London and friend of the PCs.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "The Apple Lady",
                    DescriptionShort = "A powerful Fae from the Summer Court.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Sister Walpurga",
                    DescriptionShort = "A amazonian nun and blacksmith.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Gwydion",
                    DescriptionShort = "An angel and friend.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
                new Character
                {
                    Name = "Death",
                    DescriptionShort = "The grim reaper. Sister Agnes is bound to him.",
                    CampaignId = 1, // Assuming the first campaign is the default one
                    PlayerId = 1 // Assuming the first user is the owner
                },
            };
            _dbContext.Characters.AddRange(npcs);
            _dbContext.SaveChanges();
        }
    }
}
