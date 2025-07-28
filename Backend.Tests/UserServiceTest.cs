using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;
using Backend.Services;

namespace Backend.Tests
{
    public class UserTest
    {
        CampaignToolContext GetDbContextWithData(List<User> users)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            context.Users.AddRange(users);
            context.SaveChanges();
            return context;
        }


    }
}