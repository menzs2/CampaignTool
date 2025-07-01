using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.Controllers.Tests
{
    public class ConnectionControllerTest
    {
        private static CampaignToolContext GetDbContextWithData()
        {
            var dbName = $"TestDb_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var context = new CampaignToolContext(options);
            // Seed Characters
            // Seed Campaign
            context.Campaigns.Add(new Campaign { Id = 1, CampaignName = "Test Campaign", DescriptionShort = "Short Desc", Gm = 1 });

            // Seed Connections
            context.Connections.Add(new Connection { Id = 1, ConnectionName = "Conn1", Description = "Desc", GmOnly = false, CampaignId = 1 });
            context.Connections.Add(new Connection { Id = 2, ConnectionName = "Conn2", Description = "Desc2", GmOnly = false, CampaignId = 1 }); // Added for ID mismatch test
            context.CharCharConnections.Add(new CharCharConnection { Id = 1, CharOneId = 1, CharTwoId = 2, ConnectionId = 1, Direction = 0, Description = "Desc" });
            context.CharOrgConnections.Add(new CharOrgConnection { Id = 1, CharId = 1, OrganisationId = 1, ConnectionId = 1, Direction = 1, Description = "Desc2" });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void Get_ReturnsOk_WhenConnectionsExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_ReturnsNotFound_WhenNoConnections()
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;
            var context = new CampaignToolContext(options);
            var controller = new ConnectionController(context);

            var result = controller.Get();

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_ById_ReturnsOk_WhenConnectionExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Get(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Get_ById_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Get(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Post_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new ConnectionDto { Id = 0, ConnectionName = "New", Description = "Desc", GmOnly = false, CampaignId = 1 };
            var result = controller.Post(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenNull()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_ReturnsNoContent_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new ConnectionDto { Id = 1, ConnectionName = "Updated", Description = "Desc", GmOnly = false, CampaignId = 1};
            var result = controller.Put(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenIdMismatch()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new ConnectionDto { Id = 2, ConnectionName = "Updated", Description = "Desc", GmOnly = false, CampaignId = 1 };
            var result = controller.Put(1, dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new ConnectionDto { Id = 999, ConnectionName = "Updated", Description = "Desc", GmOnly = false, CampaignId = 1 };
            var result = controller.Put(999, dto);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Delete_ReturnsBadRequest_WhenConnectionHasDependencies()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Delete(1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.Delete(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetAllCharCharConnections_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCharCharConnection_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.GetCharCharConnection(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PostCharCharConnection_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new CharCharConnectionDto { CharOneId = 1, CharTwoId = 3, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = controller.PostCharCharConnection(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void PostCharCharConnection_ReturnsBadRequest_WhenSameCharIds()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new CharCharConnectionDto { CharOneId = 1, CharTwoId = 1, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = controller.PostCharCharConnection(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PutCharCharConnection_ReturnsNoContent_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new CharCharConnectionDto { Id = 1, CharOneId = 1, CharTwoId = 2, ConnectionId = 1 };
            var result = controller.PutCharCharConnection(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCharCharConnection_ReturnsNoContent_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.DeleteCharCharConnection(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void GetAllCharOrgConnections_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.GetAllCharOrgConnections();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCharOrgConnection_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.GetCharOrgConnection(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void PostCharOrgConnection_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new CharOrgConnectionDto { CharId = 1, OrganisationId = 2, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = controller.PostCharOrgConnection(dto);

            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public void PutCharOrgConnection_ReturnsNoContent_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var dto = new CharOrgConnectionDto { Id = 1, CharId = 1, OrganisationId = 1, ConnectionId = 1, Description = "Desc", Direction =1 };
            var result = controller.PutCharOrgConnection(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCharOrgConnection_ReturnsNoContent_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionController(context);

            var result = controller.DeleteCharOrgConnection(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}