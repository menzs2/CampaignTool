
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
    }
}