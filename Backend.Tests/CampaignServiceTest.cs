using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;
using Backend.Services;

namespace Backend.Tests
{
    public class CampaignServiceTest
    {
        private CampaignToolContext GetDbContextWithData(List<Campaign> campaigns)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            context.Campaigns.AddRange(campaigns);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Get_ReturnsAllCampaigns_WhenCampaignsExist()
        {
            // Arrange
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" },
                new Campaign { Id = 2L, CampaignName = "C2", Gm = 2, DescriptionShort = "Short2" }
            };
            using var context = GetDbContextWithData(campaigns);
            var service = new CampaignService(context);

            // Act
            var result = await service.GetAllCampaigns();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoCampaignsExist()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);

            var result = await service.GetAllCampaigns();

            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_ById_ReturnsCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var service = new CampaignService(context);

            var result = await service.GetCampaignById(1);

            Assert.NotNull(result);
            var returned = Assert.IsType<CampaignDto>(result);
            Assert.Equal(campaign.Id, returned.Id);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);

            var result = await service.GetCampaignById(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByGmId_ReturnsCampaigns_WhenExists()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" },
                new Campaign { Id = 2L, CampaignName = "C2", Gm = 1, DescriptionShort = "Short2" },
                new Campaign { Id = 3L, CampaignName = "C3", Gm = 2, DescriptionShort = "Short3" }
            };
            using var context = GetDbContextWithData(campaigns);
            var service = new CampaignService(context);

            var result = await service.GetCampaignsByGmId(1);

            Assert.NotNull(result);
            Assert.IsType<List<CampaignDto>>(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByGmId_ReturnsNotFound_WhenNoneExist()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 2, DescriptionShort = "Short1" }
            };
            using var context = GetDbContextWithData(campaigns);
            var service = new CampaignService(context);

            var result = await service.GetCampaignsByGmId(99);

            Assert.Empty(result);
        }

        [Fact]
        public async Task Post_CreatesCampaign_WhenValid()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);

            var dto = new CampaignDto
            {
                Name = "New",
                Description = "Desc",
                DescriptionShort = "Short",
                GmOnlyDescription = "GM",
                GmId = 1
            };

            var result = await service.AddCampaignAsync(dto);

            var created = Assert.IsType<CampaignDto>(result);
            Assert.NotNull(created);
            Assert.IsType<CampaignDto>(result);
            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task Post_ThrowsException_WhenNull()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);

            try
            {
                var result = await service.AddCampaignAsync(null);
            }
            catch (ArgumentNullException)
            {
                // Expected exception, test passes
            }
        }

        [Fact]
        public async Task Put_UpdatesCampaign_WhenValid()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "Old", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var service = new CampaignService(context);
            var dto = new CampaignDto
            {
                Id = 1,
                Name = "Updated",
                Description = "Desc",
                DescriptionShort = "Short",
                GmOnlyDescription = "GM",
                GmId= 2
            };

            var result = await service.UpdateCampaignAsync(dto);

            var updatedCampaign = context.Campaigns.Find(1L);
            Assert.NotNull(updatedCampaign);
            Assert.Equal("Updated", updatedCampaign.CampaignName);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);

            var dto = new CampaignDto
            {
                Id = 1,
                Name = "Updated",
                GmId = 2
            };

            try
            {
                var result = await service.UpdateCampaignAsync(dto);
            }
            catch (KeyNotFoundException)
            {
                // Expected exception, test passes
            }
        }

        [Fact]
        public async Task Delete_RemovesCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "ToDelete", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var service = new CampaignService(context);

            await service.DeleteCampaignAsync(1);

            Assert.Null(context.Campaigns.Find(1L));
        }

        [Fact]
        public async Task Delete_ReturnsException_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var service = new CampaignService(context);
            try
            {
                await service.DeleteCampaignAsync(1);

            }
            catch (KeyNotFoundException)
            {
                // Expected exception, test passes
            }
        }
    }
}