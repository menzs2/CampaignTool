using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend
{
    public class UserService
    {
        private readonly CampaignToolContext _context;

        public UserService(CampaignToolContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.ToDto();
        }

        public async Task<UserDto?> GetUserByID(long id)
        {
            var user = await _context.Users.FindAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDto?> CreateUser(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "Character data is null.");
            }
            var newUser = userDto.ToModel();
            if (newUser == null)
            {
                throw new InvalidOperationException("Failed to convert UserDTo to User model.");
            }
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            // Automatically create a UserSetting for the new user
            var usersetting = new UserSetting
            {
                UserId = newUser.Id,
                DefaultCampaignId = null,
                SelectLastCampaign = true,
                SameNameWarning = true
            };
            _context.UserSettings.Add(usersetting);
            await _context.SaveChangesAsync();
            return newUser?.ToDto();
        }

        public async Task UpdateUser(long id, UserDto userDto)
        {
            var exisitingUser = await _context.Users.FindAsync(id);
            if (exisitingUser == null)
            {
                throw new KeyNotFoundException($"User with id '{id}' not found.");
            }
            exisitingUser.FirstName = userDto.FirstName;
            exisitingUser.LastName = userDto.LastName;
            exisitingUser.HasLogin = userDto.HasLogin;
            _context.Update(exisitingUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(long id)
        {
            var exisitingUser = await _context.Users.FindAsync(id);
            if (exisitingUser == null)
            {
                throw new KeyNotFoundException($"User with id '{id}' not found.");
            }
            _context.Remove(exisitingUser);
            await _context.SaveChangesAsync();
        }
    }
}
