
using Backend.Models;
using Backend.Data;
using Shared;

namespace Backend.Tests
{
    public class ExtensionsTest
    {
        [Fact]
        public void ToDto_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            var campaigns = new List<Campaign>();

            // Act
            var result = campaigns.ToDto();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToDto_WithNullCampaigns_IgnoresNulls()
        {
            // Arrange
            var campaigns = new List<Campaign?> { null, null };

            // Act
            var result = campaigns.ToDto();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToDto_WithValidCampaigns_ReturnsDtoList()
        {
            // Arrange
            var campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Id = 1,
                    CampaignName = "Test Campaign",
                    DescriptionShort = "Short",
                    Description = "Long",
                    Gm = 1,
                    GmOnlyDescription = "GM Only"
                },
                new Campaign
                {
                    Id = 2,
                    CampaignName = "Another Campaign",
                    DescriptionShort = "Short2",
                    Description = "Long2",
                    Gm = 2,
                    GmOnlyDescription = "GM Only 2"
                }
            };

            // Act
            var result = campaigns.ToDto();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            var resultList = result.ToList();
            Assert.Equal("Test Campaign", resultList[0].Name);
            Assert.Equal("Another Campaign", resultList[1].Name);
        }

        [Fact]
        public void ToDto_WithMixedNullAndValidCampaigns_SkipsNulls()
        {
            // Arrange
            var campaigns = new List<Campaign?>
            {
                null,
                new Campaign
                {
                    Id = 3,
                    CampaignName = "Valid",
                    DescriptionShort = "Short",
                    Description = "Desc",
                    Gm = 1,
                    GmOnlyDescription = "GM Only"
                }
            };

            // Act
            var result = ((IEnumerable<Campaign>)campaigns).ToDto();

            // Assert
            Assert.Single(result);
            var resultList = result.ToList();
            Assert.Equal(3, resultList[0].Id);
        }
        [Fact]
        public void Character_ToDto_WithNullCharacter_ReturnsNull()
        {
            // Arrange
            Character? character = null;

            // Act
            var result = character.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Character_ToDto_WithValidCharacter_ReturnsDtoWithSameValues()
        {
            // Arrange
            var character = new Character
            {
                Id = 10,
                Name = "Hero",
                Description = "A brave hero",
                CampaignId = 5,
                PlayerId = 2,
                DescriptionShort = "Short desc"
            };

            // Act
            var dto = character.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(character.Id, dto.Id);
            Assert.Equal(character.Name, dto.Name);
            Assert.Equal(character.Description, dto.Description);
            Assert.Equal(character.CampaignId, dto.CampaignId);
            Assert.Equal(character.PlayerId, dto.PlayerId);
            Assert.Equal(character.DescriptionShort, dto.DescriptionShort);
        }


        [Fact]
        public void Organisation_ToDto_WithNullOrganisation_ReturnsNull()
        {
            // Arrange
            Organisation? organisation = null;

            // Act
            var result = organisation.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Organisation_ToDto_WithValidOrganisation_ReturnsDtoWithSameValues()
        {
            // Arrange
            var organisation = new Organisation
            {
                Id = 42,
                Name = "Guild of Heroes",
                DescriptionShort = "Short desc",
                Description = "A guild for heroes.",
                State = "Active",
                CampaignId = 7,
                GmOnly = true,
                GmOnlyDescription = "Secret info"
            };

            // Act
            var dto = organisation.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(organisation.Id, dto.Id);
            Assert.Equal(organisation.Name, dto.Name);
            Assert.Equal(organisation.DescriptionShort, dto.DescriptionShort);
            Assert.Equal(organisation.Description, dto.Description);
            Assert.Equal(organisation.State, dto.State);
            Assert.Equal(organisation.CampaignId, dto.CampaignId);
            Assert.Equal(organisation.GmOnly, dto.GmOnly);
            Assert.Equal(organisation.GmOnlyDescription, dto.GmOnlyDescription);
        }
        [Fact]
        public void User_ToDto_WithNullUser_ReturnsNull()
        {
            // Arrange
            User? user = null;

            // Act
            var result = user.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void User_ToDto_WithValidUser_ReturnsDtoWithSameValues()
        {
            // Arrange
            var user = new User
            {
                Id = 100,
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = "john@example.com",
                HasLogin = true,
                Password = "password123",
                Role = 1
            };

            // Act
            var dto = user.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.UserName, dto.UserName);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.HasLogin, dto.HasLogin);
            Assert.Equal(user.Password, dto.Password);
            Assert.Equal(user.Role, dto.Role);
        }


        [Fact]
        public void UserSetting_ToDto_WithNullUserSetting_ReturnsNull()
        {
            // Arrange
            UserSetting? userSetting = null;

            // Act
            var result = userSetting.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UserSetting_ToDto_WithValidUserSetting_ReturnsDtoWithSameValues()
        {
            // Arrange
            var userSetting = new UserSetting
            {
                Id = 55,
                SelectLastCampaign = true,
                SameNameWarning = false,
                DefaultCampaignId = 123
            };

            // Act
            var dto = userSetting.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(userSetting.Id, dto.Id);
            Assert.Equal(userSetting.SelectLastCampaign, dto.SelectLastCampaign);
            Assert.Equal(userSetting.SameNameWarning, dto.SameNameWarning);
            Assert.Equal(userSetting.DefaultCampaignId, dto.DefaultCampaignId);
        }

        [Fact]
        public void ToDto_WithNullUser_ReturnsNull()
        {
            // Arrange
            User? user = null;

            // Act
            var result = user.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToDto_WithValidUser_ReturnsDtoWithSameValues()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Smith",
                UserName = "asmith",
                Email = "alice@example.com",
                HasLogin = true,
                Password = "securepassword",
                Role = 2
            };

            // Act
            var dto = user.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.FirstName, dto.FirstName);
            Assert.Equal(user.LastName, dto.LastName);
            Assert.Equal(user.UserName, dto.UserName);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.HasLogin, dto.HasLogin);
            Assert.Equal(user.Password, dto.Password);
            Assert.Equal(user.Role, dto.Role);
        }

        [Fact]
        public void CharCharConnection_ToDto_WithNullConnection_ReturnsNull()
        {
            // Arrange
            CharCharConnection? connection = null;

            // Act
            var result = connection.ToDto();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CharCharConnection_ToDto_WithValidConnection_ReturnsDtoWithSameValues()
        {
            // Arrange
            var connection = new CharCharConnection
            {
                Id = 1,
                Description = "A strong bond between characters.",
                GmOnly = false,
                GmOnlyDescription = "Visible only to GM",
                CharOneId = 10,
                CharTwoId = 20,
                ConnectionId = 5
            };

            // Act
            var dto = connection.ToDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(connection.Id, dto.Id);
            Assert.Equal(connection.Description, dto.Description);
            Assert.Equal(connection.GmOnly, dto.GmOnly);
            Assert.Equal(connection.GmOnlyDescription, dto.GmOnlyDescription);
            Assert.Equal(connection.CharOneId, dto.CharOneId);
            Assert.Equal(connection.CharTwoId, dto.CharTwoId);
            Assert.Equal(connection.ConnectionId, dto.ConnectionId);
        }

        [Fact]
       public void CharCharConnection_ToModel_WithNullDto_ReturnsNull()
        {
            // Arrange
            CharCharConnectionDto? dto = null;

            // Act
            var result = dto.ToModel();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CharCharConnection_ToModel_WithValidDto_ReturnsModelWithSameValues()
        {
            // Arrange
            var dto = new CharCharConnectionDto
            {
                Id = 1,
                Description = "A strong bond between characters.",
                GmOnly = false,
                GmOnlyDescription = "Visible only to GM",
                CharOneId = 10,
                CharTwoId = 20,
                ConnectionId = 5
            };

            // Act
            var model = dto.ToModel();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Description, model.Description);
            Assert.Equal(dto.GmOnly, model.GmOnly);
            Assert.Equal(dto.GmOnlyDescription, model.GmOnlyDescription);
            Assert.Equal(dto.CharOneId, model.CharOneId);
            Assert.Equal(dto.CharTwoId, model.CharTwoId);
            Assert.Equal(dto.ConnectionId, model.ConnectionId);
        }

    }
}