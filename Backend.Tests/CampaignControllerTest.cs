using Xunit;
using Backend.Controllers;
using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Tests
{
    public class CampaignControllerTest
    {
        private CampaignToolContext GetDbContextWithData(List<Campaign> campaigns)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "CampaignTestDb" + System.Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            context.Campaigns.AddRange(campaigns);
            context.SaveChanges();
            return context;
        }

        private CampaignDto ToDto(Campaign c) => new CampaignDto
        {
            Id = c.Id,
            CampaignName = c.CampaignName,
            Description = c.Description,
            DescriptionShort = c.DescriptionShort,
            GmOnlyDescription = c.GmOnlyDescription,
            Gm = c.Gm
        };

        [Fact]
        public void Get_ReturnsAllCampaigns_WhenCampaignsExist()
        {
            // Arrange
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" },
                new Campaign { Id = 2L, CampaignName = "C2", Gm = 2, DescriptionShort = "Short2" }
            };
            using var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<CampaignDto>>(okResult.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenNoCampaignsExist()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = controller.Get();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_ById_ReturnsCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var result = controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<CampaignDto>(okResult.Value);
            Assert.Equal(campaign.Id, returned.Id);
        }

        [Fact]
        public void Get_ById_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = controller.Get(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetByGmId_ReturnsCampaigns_WhenExists()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 1, DescriptionShort = "Short1" },
                new Campaign { Id = 2L, CampaignName = "C2", Gm = 1, DescriptionShort = "Short2" },
                new Campaign { Id = 3L, CampaignName = "C3", Gm = 2, DescriptionShort = "Short3" }
            };
            using var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            var result = controller.GetByGmId(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsAssignableFrom<IEnumerable<CampaignDto>>(okResult.Value);
            Assert.Equal(2, returned.Count());
        }

        [Fact]
        public void GetByGmId_ReturnsNotFound_WhenNoneExist()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1L, CampaignName = "C1", Gm = 2, DescriptionShort = "Short1" }
            };
            using var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            var result = controller.GetByGmId(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Post_CreatesCampaign_WhenValid()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var dto = new CampaignDto
            {
                CampaignName = "New",
                Description = "Desc",
                DescriptionShort = "Short",
                GmOnlyDescription = "GM",
                Gm = 1
            };

            var result = await controller.Post(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", created.ActionName);
            Assert.NotNull(context.Campaigns.FirstOrDefault(c => c.CampaignName == "New"));
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenNull()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = await controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_UpdatesCampaign_WhenValid()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "Old", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var dto = new CampaignDto
            {
                Id = 1,
                CampaignName = "Updated",
                Description = "Desc",
                DescriptionShort = "Short",
                GmOnlyDescription = "GM",
                Gm = 2
            };

            var result = await controller.Put(1, dto);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated", context.Campaigns.Find(1L).CampaignName);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenIdMismatch()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "Old", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var dto = new CampaignDto
            {
                Id = 2,
                CampaignName = "Updated",
                Gm = 2
            };

            var result = await controller.Put(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var dto = new CampaignDto
            {
                Id = 1,
                CampaignName = "Updated",
                Gm = 2
            };

            var result = await controller.Put(1, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_RemovesCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1L, CampaignName = "ToDelete", Gm = 1, DescriptionShort = "Short1" };
            using var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Campaigns.Find(1L));
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            using var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}