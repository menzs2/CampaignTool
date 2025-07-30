using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests;

public class OrganisationServiceTest
{
    private static CampaignToolContext GetDbContextWithData(List<Organisation>? organisations = null)
    {
        var options = new DbContextOptionsBuilder<CampaignToolContext>()
            .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
            .Options;
        var context = new CampaignToolContext(options);

        context.Campaigns.Add(new Campaign { Id = 1L, CampaignName = "Test Campaign", DescriptionShort = "Short Desc", Gm = 1 });
        context.Campaigns.Add(new Campaign { Id = 2L, CampaignName = "Test Campaign 2", DescriptionShort = "Short Desc", Gm = 1 });

        //Seed Organisations
        if (organisations == null)
        {
            context.Organisations.Add(new Organisation { Id = 1L, Name = "OrgaOne", DescriptionShort = "OrgaOneShort", CampaignId = 1L });
            context.Organisations.Add(new Organisation { Id = 2L, Name = "OrgaTwo", DescriptionShort = "OrgaOneShort", CampaignId = 1L });
            context.Organisations.Add(new Organisation { Id = 3L, Name = "OrgaThree", DescriptionShort = "OrgaOneShort", CampaignId = 2L });
            // Seed Connections
            context.Connections.Add(new Connection { Id = 1, ConnectionName = "Conn1", Description = "Desc", GmOnly = false, CampaignId = 1 });
            context.Connections.Add(new Connection { Id = 2, ConnectionName = "Conn2", Description = "Desc2", GmOnly = false, CampaignId = 1 }); // Added for ID mismatch test
            context.OrgOrgConnections.Add(new OrgOrgConnection { Id = 1, OrgOneId = 1, OrgTwoId = 2, ConnectionId = 1, Direction = 0, Description = "Desc" });
        }
        else
        {
            context.Organisations.AddRange(organisations);
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Get_ReturnsEmpty_WhenNotExists()
    {
        var context = GetDbContextWithData(new List<Organisation>());
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationDtoAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAll_ReturnsList_WhenExists()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationDtoAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<List<OrganisationDto>?>(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByCampaingID_ReturnOrganisations_WhenExists()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationDtoAsync(1);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<List<OrganisationDto>?>(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByCampaignID_ReturnEmpty_WhenNotFound()
    {
        var context = GetDbContextWithData(new List<Organisation>());
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationDtoAsync(99);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByID_ReturnsObject_WhenExists()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationDtoByIdAsync(2);
        Assert.NotNull(result);
        Assert.IsType<OrganisationDto?>(result);
        Assert.Equal("OrgaTwo", result.Name);
    }

    [Fact]
    public async Task GetById_ReturnException_WhenNotFound()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        try
        {
            var result = await service.GetOrganisationDtoByIdAsync(999);
        }
        catch (KeyNotFoundException)
        {
            // Expected exception, test passes
        }
    }

    [Fact]
    public async Task GetByName_ReturnOrga_WhenFound()
    {
        var nameToSearch = "OrgaThree";
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetOrganisationsByNameAsync(nameToSearch);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.IsType<List<OrganisationDto>>(result);
        Assert.Single(result);
        Assert.Equal(nameToSearch, result.FirstOrDefault()?.Name);

    }

    [Fact]
    public async Task GetByName_ReturnException_WhenNotFound()
    {
        var nameToSearch = "OrgaFour";
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        try
        {
            var result = await service.GetOrganisationsByNameAsync(nameToSearch);
        }
        catch (KeyNotFoundException)
        {
            // Expected exception, test passes
        }
    }

    [Fact]
    public async Task GetConnected_ReturnOrganisation_WhenFound()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetConnectedOrganisationsAsync(1);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(2, result.FirstOrDefault()?.Id);
    }

    [Fact]
    public async Task GetConnected_ReturnEmpty_WhenNotFound()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        var result = await service.GetConnectedOrganisationsAsync(3);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);
        var newOrganisation = new OrganisationDto
        {
            Name = "OrgaFour",
            CampaignId = 1,
            DescriptionShort = "Organisation Four"
        };

        var result = await service.CreateOrganisation(newOrganisation);
        var OrgaFour = await service.GetOrganisationDtoByIdAsync(4);

        Assert.IsType<OrganisationDto>(result);
        Assert.Equal(newOrganisation.Name, result.Name);
        Assert.IsType<OrganisationDto>(OrgaFour);
        Assert.NotNull(result.Id);
        Assert.Equal(newOrganisation.Name, OrgaFour.Name);
    }

    [Fact]
    public async Task CreateOrganisation_ReturnException_WhenNotValid()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);
        try
        {
            var result = await service.CreateOrganisation(null);
        }
        catch (ArgumentNullException)
        {
            // Expected exception, test passes
        }
    }

    [Fact]
    public async Task UpdateOrganisation_Success_WhenValid()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);
        var organisationToUpdate = new OrganisationDto { Id = 2L, Name = "OrgaTwo", DescriptionShort = "OrgaTowShort", CampaignId = 1L };

        await service.UpdateOrganisation(2, organisationToUpdate);
        var updatedOrganisation = await service.GetOrganisationDtoByIdAsync(2);

        Assert.IsType<OrganisationDto>(updatedOrganisation);
        Assert.Equal(organisationToUpdate.DescriptionShort, updatedOrganisation.DescriptionShort);
        Assert.Equal(organisationToUpdate.Name, updatedOrganisation.Name);
    }

    [Fact]
    public async Task UpdateOrganisation_ReturnException_WhenNotExistin()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);
        var organisationToUpdate = new OrganisationDto { Id = 4L, Name = "OrgaFour", DescriptionShort = "OrgaFourShort", CampaignId = 1L };

        try
        {
            await service.UpdateOrganisation(2, organisationToUpdate);
        }
        catch (KeyNotFoundException)
        {
            // Expected exception, test passes
        }
    }

    [Fact]
    public async Task DeleteOrganisation_Success_WhenValid()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        await service.DeleteOrganisation(3);

        var organisations = await service.GetOrganisationDtoAsync();
        Assert.IsType<List<OrganisationDto>>(organisations);
        Assert.Equal(2, organisations?.Count());
    }

    [Fact]
    public async Task DeleteOrtanisation_ReturnException_WhenNotFound()
    {
        var context = GetDbContextWithData();
        var service = new OrganisationService(context);

        try
        {
            await service.DeleteOrganisation(999);
        }
        catch (KeyNotFoundException)
        {
            // Expected exception, test passes
        }
    }
}
