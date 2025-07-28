using Backend.Data;
using Backend.Models;
using Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Backend.Tests
{
    public class UserServiceTest
    {
        private List<User> Users => new List<User>
        {
            new User { Id = 1L,
                FirstName = "Jean-Helmut",
                LastName = "Muster",
                Email = "j.m@example.com",
                UserName = "jhm",
                HasLogin = true, Password = "Password123",
                UserSetting =  new UserSetting { Id = 1L, UserId = 1, DefaultCampaignId = null, SameNameWarning = true, SelectLastCampaign = true }},
            new User { Id = 2L,
                FirstName = "Phillippa",
                LastName = "Marker",
                UserName = "pm",
                Email = "P.m@example.com",
                HasLogin = true,
                Password = "Password456",
                UserSetting = new UserSetting { Id = 2L, UserId = 2, DefaultCampaignId = null, SameNameWarning = false, SelectLastCampaign = true } }
        };


        CampaignToolContext GetDbContextWithData(List<User> users)
        {
            var options = new DbContextOptionsBuilder<CampaignToolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid())
                .Options;
            var context = new CampaignToolContext(options);
            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Get_ReturnsUser_WhenExist()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);

            var result = await service.GetAllUsers();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<UserDto>>(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Get_ReturnEmpty_WhenNotExist()
        {
            var context = GetDbContextWithData(new List<User> { });
            var service = new UserService(context);

            var result = await service.GetAllUsers();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetById_ReturnUser_WhenExist()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);

            var result = await service.GetUserByID(1);

            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal(Users[0].LastName, result.LastName);
        }

        [Fact]
        public async Task GetById_ReturnNull_WhenNotExtist()
        {
            var context = GetDbContextWithData(new List<User> { });
            var service = new UserService(context);

            var result = await service.GetUserByID(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ReturnObject_WhenValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            var newUser = new User
            {
                FirstName = "Alan",
                LastName = "marker",
                Email = "a.m@example.com",
                UserName = "am",
                HasLogin = true,
                Password = "Password789",
                UserSetting = new UserSetting { Id = 3L, UserId = 3, DefaultCampaignId = null, SameNameWarning = true, SelectLastCampaign = true }
            };
            var result = await service.CreateUser(newUser.ToDto());

            Assert.NotNull(result);
            Assert.Equal(result.LastName, newUser.LastName);
        }

        [Fact]
        public async Task CreateUser_ReturnUserSetting_WhenCreated()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            var newUser = new User
            {
                FirstName = "Alan",
                LastName = "marker",
                Email = "a.m@example.com",
                UserName = "am",
                HasLogin = true,
                Password = "Password789",
                UserSetting = new UserSetting { Id = 3L, UserId = 3, DefaultCampaignId = null, SameNameWarning = true, SelectLastCampaign = true }
            };
            await service.CreateUser(newUser.ToDto());
            var result = await service.GetUserSettingById(3);

            Assert.NotNull(result);
            Assert.IsType<UserSettingDto>(result);
            Assert.Equal(result.SameNameWarning, newUser.UserSetting.SameNameWarning);
        }

        [Fact]
        public async Task CreateUser_ReturnError_WhenNotValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            try
            {
                var result = await service.CreateUser(null);
            }
            catch (ArgumentNullException)
            {
                // Expected exception, test passes
            }
        }

        [Fact]
        public async Task UpateUser_Success_WhenValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);

            var userToUpdate = new UserDto
            {
                Id = 1L,
                FirstName = "Jean-Helmut",
                LastName = "Example",
                Email = "j.m@example.com",
                UserName = "jhm",
                HasLogin = true,
                Password = "Password123",
            };
            await service.UpdateUser(userToUpdate.Id.Value, userToUpdate);

            var updatedUsers = await service.GetUserByID(1);
            Assert.NotNull(updatedUsers);
            Assert.IsType<UserDto>(updatedUsers);
            Assert.Equal(updatedUsers.LastName, userToUpdate.LastName);
        }

        [Fact]
        public async Task UpdateUser_ReturnException_WhenNotValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);

            var userToUpdate = new UserDto
            {
                Id = 4L,
                FirstName = "Jean-Helmut",
                LastName = "Example",
                Email = "j.m@example.com",
                UserName = "jhm",
                HasLogin = true,
                Password = "Password123",
            };
            try
            {
                await service.UpdateUser(userToUpdate.Id.Value, userToUpdate);
            }
            catch (KeyNotFoundException)
            {
                // Expected exception, test passes
            }
        }

        [Fact]
        public async Task DeleteUser_Succes_WhenValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            await service.DeleteUser(1);

            var result = await service.GetAllUsers();

            Assert.IsType<List<UserDto>>(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Null(result.FirstOrDefault(r => r.Id == 1));
        }

        [Fact]
        public async Task DeleteUser_ReuturnException_WhenNoValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            try
            {
                await service.DeleteUser(99);
            }

            catch (KeyNotFoundException)
            {
                // Expected exception, test passes
            }
        }

        [Fact]
        public async Task UpdateUserSetting_Success_WhenValid()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            var settingBeforeUpdate = await service.GetUserSettingById(2);
            var userSettingToUpdate = new UserSettingDto
            {
                Id = 2L,
                UserId = 2,
                DefaultCampaignId = null,
                SameNameWarning = true,
                SelectLastCampaign = false
            };
            await service.UpdateUserSetting(userSettingToUpdate.Id.Value, userSettingToUpdate);

            var settingAfterUpdate = await service.GetUserSettingById(2);
            Assert.IsType<UserSettingDto>(settingAfterUpdate);
            Assert.IsType<UserSettingDto>(settingBeforeUpdate);
            Assert.NotEqual(userSettingToUpdate.SameNameWarning, settingBeforeUpdate.SameNameWarning);
            Assert.NotEqual(userSettingToUpdate.SelectLastCampaign, settingBeforeUpdate.SelectLastCampaign);

        }

        [Fact]
        public async Task DeleteUserSettingOnDeleteUser()
        {
            var context = GetDbContextWithData(Users);
            var service = new UserService(context);
            await service.DeleteUser(1);
            try
            {
                await service.GetUserSettingById(1);
            }
            catch (KeyNotFoundException)
            {
                // Expected exception, test passes
            }
        }
    }
}