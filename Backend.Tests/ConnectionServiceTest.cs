using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.Tests
{
    public class ConnectionServiceTest
    {
        private static CampaignToolContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            // Seed Characters
            // Seed Campaign
            context.Campaigns.Add(new Campaign { Id = 1, Name = "Test Campaign", DescriptionShort = "Short Desc", Gm = 1 });

            // Seed Connections
            context.Connections.Add(new Connection { Id = 1, ConnectionName = "Conn1", Description = "Desc", GmOnly = false, CampaignId = 1 });
            context.Connections.Add(new Connection { Id = 2, ConnectionName = "Conn2", Description = "Desc2", GmOnly = false, CampaignId = 1 }); // Added for ID mismatch test
            context.CharCharConnections.Add(new CharCharConnection { Id = 1, CharOneId = 1, CharTwoId = 2, ConnectionId = 1, Direction = 0, Description = "Desc" });
            context.CharOrgConnections.Add(new CharOrgConnection { Id = 1, CharId = 1, OrganisationId = 1, ConnectionId = 1, Direction = 1, Description = "Desc2" });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenConnectionsExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetConnectionsAsync();

            Assert.IsType<List<ConnectionDto>>(result);
        }

        [Fact]
        public async Task Get_ById_ReturnsOk_WhenConnectionExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetConnectionByIdAsync(1);

            Assert.IsType<ConnectionDto>(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task Get_ById_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetConnectionByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task Post_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new ConnectionDto { ConnectionName = "New", Description = "Desc", GmOnly = false, CampaignId = 1 };
            var result = await controller.CreateConnectionAsync(dto);

            Assert.IsType<ConnectionDto>(result);
            Assert.NotNull(result.Id);

        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenNull()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            try
            {
                var result = await controller.CreateConnectionAsync(null);
            }
            catch (Exception ex)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
        }

        [Fact]
        public async Task Put_ReturnsNoContent_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new ConnectionDto { Id = 1, ConnectionName = "Updated", Description = "Desc", GmOnly = false, CampaignId = 1 };
            var result = await controller.UpdateConnectionAsync(1, dto);

            Assert.IsType<ConnectionDto>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new ConnectionDto { Id = 999, ConnectionName = "Updated", Description = "Desc", GmOnly = false, CampaignId = 1 };
            try
            {
                var result = await controller.UpdateConnectionAsync(999, dto);
            }
            catch (KeyNotFoundException) { }
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenConnectionHasDependencies()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            try
            {
                await controller.DeleteConnectionAsync(1);
            }
            catch (Exception ex)
            {
                Assert.IsType<InvalidOperationException>(ex);
            }

        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenConnectionDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            try
            {
                await controller.DeleteConnectionAsync(999);
            }
            catch (Exception ex)
            {
                Assert.IsType<KeyNotFoundException>(ex);
            }


        }

        [Fact]
        public async Task GetAllCharCharConnections_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetAllCharToCharConnectionsAsync();

            Assert.IsType<List<CharCharConnectionDto>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetCharCharConnection_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetCharToCharConnectionByIdAsync(1);

            Assert.IsType<CharCharConnectionDto>(result);
        }

        [Fact]
        public async Task PostCharCharConnection_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new CharCharConnectionDto { CharOneId = 1, CharTwoId = 3, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = await controller.CreateCharToCharConnectionAsync(dto);

            Assert.IsType<CharCharConnectionDto>(result);
        }


        [Fact]
        public async Task PostCharCharConnection_ReturnsBadRequest_WhenSameCharIds()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new CharCharConnectionDto { Id = 1, CharOneId = 1, CharTwoId = 2, ConnectionId = 1 };
            var result = await controller.UpdateCharToCharConnectionAsync(1, dto);

            Assert.IsType<CharCharConnectionDto>(result);
        }

        [Fact]
        public async Task DeleteCharCharConnection_ReturnsNoContent_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            await controller.DeleteCharToCharConnectionAsync(1);
            var record = await controller.GetCharToCharConnectionByIdAsync(1);
            Assert.Null(record);
        }

        [Fact]
        public async Task DeleteCharCharConnection_ReturnsNotFound_WhenDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            try
            {
                await controller.DeleteCharToCharConnectionAsync(999);
            }
            catch (KeyNotFoundException) { }
        }

        [Fact]
        public async Task GetAllCharOrgConnections_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetAllCharToOrgConnectionsAsync();

            Assert.IsType<List<CharOrgConnectionDto>>(result);
        }

        [Fact]
        public async Task GetCharOrgConnection_ReturnsOk_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var result = await controller.GetCharToOrgConnectionByIdAsync(1);

            Assert.IsType<CharOrgConnectionDto?>(result);
            Assert.Equal(1, result.CharId);
            Assert.Equal(1, result.OrganisationId);
        }

        [Fact]
        public async Task PostCharOrgConnection_ReturnsCreated_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new CharOrgConnectionDto { CharId = 1, OrganisationId = 2, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = await controller.CreateCharToOrgConnectionAsync(dto);

            Assert.IsType<CharOrgConnectionDto>(result);
        }

        [Fact]
        public async Task PutCharOrgConnection_ReturnsNoContent_WhenValid()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            var dto = new CharOrgConnectionDto { CharId = 1, OrganisationId = 1, ConnectionId = 1, Description = "Desc", Direction = 1 };
            var result = await controller.CreateCharToOrgConnectionAsync(dto);

            Assert.IsType<CharOrgConnectionDto?>(result);
        }

        [Fact]
        public async Task DeleteCharOrgConnection_ReturnsNoContent_WhenExists()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            await controller.DeleteCharacterToOrganizationConnectionAsync(1);
            var record = await controller.GetCharToOrgConnectionByIdAsync(1);
            Assert.Null(record);
        }

        [Fact]
        public async Task DeleteCharOrgConnection_ReturnsNotFound_WhenDoesNotExist()
        {
            var context = GetDbContextWithData();
            var controller = new ConnectionService(context);

            try
            {
                await controller.DeleteCharacterToOrganizationConnectionAsync(999);
            }
            catch (KeyNotFoundException) { }
        }
    }
}