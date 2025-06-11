
using Backend.Models;
using Shared;

namespace Backend.Data.Tests
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
            var result = ((IEnumerable<Campaign>)campaigns).ToDto();

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
            Assert.Equal(2, result.Count);
            Assert.Equal("Test Campaign", result[0].CampaignName);
            Assert.Equal("Another Campaign", result[1].CampaignName);
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
            Assert.Equal(3, result[0].Id);
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
                CharacterName = "Hero",
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
            Assert.Equal(character.CharacterName, dto.CharacterName);
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
                OrganisationName = "Guild of Heroes",
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
            Assert.Equal(organisation.OrganisationName, dto.OrganisationName);
            Assert.Equal(organisation.DescriptionShort, dto.DescriptionShort);
            Assert.Equal(organisation.Description, dto.Description);
            Assert.Equal(organisation.State, dto.State);
            Assert.Equal(organisation.CampaignId, dto.CampaignId);
            Assert.Equal(organisation.GmOnly, dto.GmOnly);
            Assert.Equal(organisation.GmOnlyDescription, dto.GmOnlyDescription);
        }
    }
}