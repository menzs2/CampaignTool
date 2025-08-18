using Backend.Data;
using Backend.Models;
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
            var users = await _context.Player.ToListAsync();
            return users.ToDto();
        }

        public async Task<UserDto?> GetUserByID(long id)
        {
            var user = await _context.Player.FindAsync(id);
            return user?.ToDto();
        }

        public async Task<UserDto?> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            var user = await _context.Player.FirstOrDefaultAsync(u => u.Email == email);
            return user?.ToDto();
        }

        public async Task<UserDto?> GetUserByLogin(string id)
        {
            var user = await _context.Player.FirstOrDefaultAsync(u => u.AppUserId == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }
            return user.ToDto();
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
            _context.Player.Add(newUser);
            await _context.SaveChangesAsync();
            await CreateUserSetting(newUser.Id);
            return newUser?.ToDto();
        }

        public async Task UpdateUser(long id, UserDto userDto)
        {
            var exisitingUser = await _context.Player.FindAsync(id);
            if (exisitingUser == null)
            {
                throw new KeyNotFoundException($"User with id '{id}' not found.");
            }
            exisitingUser.FirstName = userDto.FirstName;
            exisitingUser.LastName = userDto.LastName;
            exisitingUser.HasLogin = userDto.HasLogin;
            exisitingUser.Role = userDto.Role;
            _context.Update(exisitingUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(long id)
        {
            var exisitingUser = await _context.Player.FindAsync(id);
            if (exisitingUser == null)
            {
                throw new KeyNotFoundException($"User with id '{id}' not found.");
            }
            _context.Remove(exisitingUser);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserSettingDto>?> GetUserSettings()
        {
            var userSettings = await _context.UserSettings.ToListAsync();
            return userSettings?.ToDto();
        }

        public async Task<UserSettingDto?> GetUserSettingById(long id)
        {
            var userSetting = await _context.UserSettings.FindAsync(id);
            if (userSetting == null)
            {
                throw new KeyNotFoundException("User setting not found");
            }
            return userSetting?.ToDto();
        }

        public async Task<UserSettingDto> CreateUserSetting(long userId, UserSettingDto? userSettingDto = null)
        {
            var newSetting = userSettingDto ?? new UserSettingDto
            {
                UserId = userId,
                DefaultCampaignId = null,
                SelectLastCampaign = true,
                SameNameWarning = true
            };
            if (_context.UserSettings.FirstOrDefault(r => r.UserId == userId) is UserSetting existingUserSetting)
            {
                return existingUserSetting.ToDto();
            }
            _context.UserSettings.Add(newSetting.ToModel());
            await _context.SaveChangesAsync();
            return newSetting;
        }

        public async Task UpdateUserSetting(long id, UserSettingDto userSetting)
        {
            var exisitingUserSetting = await _context.UserSettings.FirstOrDefaultAsync(u => u.UserId == id);
            if (exisitingUserSetting == null)
            {
                throw new KeyNotFoundException($"User setting for user with id '{id}'");
            }
            exisitingUserSetting.DefaultCampaignId = userSetting.DefaultCampaignId;
            exisitingUserSetting.SelectLastCampaign = userSetting.SelectLastCampaign;
            exisitingUserSetting.SameNameWarning = userSetting.SameNameWarning;
            _context.Update(exisitingUserSetting);
            await _context.SaveChangesAsync();
        }


    }
}
