using Backend.Data;
using Backend.Models; // Ensure this is imported
using Shared; // Ensure this is imported if CampaignDto is here
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Controllers;

namespace Backend.Tests // Update namespace to match folder structure
{
    public class CampaignControllerTest
    {
        private static CampaignToolContext GetDbContextWithData(List<Campaign> campaigns)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "CampaignTestDb" + System.Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            context.Campaigns.AddRange(campaigns);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void Get_ReturnsAllCampaigns_WhenCampaignsExist()
        {
            // Arrange
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1, CampaignName = "Test1", Gm = 1 },
                new Campaign { Id = 2, CampaignName = "Test2", Gm = 2 }
            };
            var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCampaigns = Assert.IsAssignableFrom<IEnumerable<CampaignDto>>(okResult.Value);
            Assert.Equal(2, returnedCampaigns.Count());
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenNoCampaignsExist()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = controller.Get();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_ById_ReturnsCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1, CampaignName = "Test", Gm = 1 };
            var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var result = controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCampaign = Assert.IsType<CampaignDto>(okResult.Value);
            Assert.Equal(1, returnedCampaign.Id);
        }

        [Fact]
        public void Get_ById_ReturnsNotFound_WhenNotExists()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = controller.Get(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetByGmId_ReturnsCampaigns_WhenExist()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1, CampaignName = "Test1", Gm = 1 },
                new Campaign { Id = 2, CampaignName = "Test2", Gm = 1 }
            };
            var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            var result = controller.GetByGmId(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCampaigns = Assert.IsAssignableFrom<IEnumerable<CampaignDto>>(okResult.Value);
            Assert.Equal(2, returnedCampaigns.Count());
        }

        [Fact]
        public void GetByGmId_ReturnsNotFound_WhenNoneExist()
        {
            var campaigns = new List<Campaign>
            {
                new Campaign { Id = 1, CampaignName = "Test1", Gm = 2 }
            };
            var context = GetDbContextWithData(campaigns);
            var controller = new CampaignController(context);

            var result = controller.GetByGmId(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);
            var dto = new CampaignDto { Id = 0, CampaignName = "New", Gm = 1 };

            var result = await controller.Post(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenNull()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = await controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_UpdatesCampaign_WhenValid()
        {
            var campaign = new Campaign { Id = 1, CampaignName = "Old", Gm = 1 };
            var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);
            var dto = new CampaignDto { Id = 1, CampaignName = "Updated", Gm = 1 };

            var result = await controller.Put(1, dto);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated", context.Campaigns.Find(1).CampaignName);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenIdMismatch()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);
            var dto = new CampaignDto { Id = 2, CampaignName = "Test", Gm = 1 };

            var result = await controller.Put(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenCampaignNotExist()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);
            var dto = new CampaignDto { Id = 1, CampaignName = "Test", Gm = 1 };

            var result = await controller.Put(1, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_RemovesCampaign_WhenExists()
        {
            var campaign = new Campaign { Id = 1, CampaignName = "Test", Gm = 1 };
            var context = GetDbContextWithData(new List<Campaign> { campaign });
            var controller = new CampaignController(context);

            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Campaigns.Find(1));
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenNotExists()
        {
            var context = GetDbContextWithData(new List<Campaign>());
            var controller = new CampaignController(context);

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }

}