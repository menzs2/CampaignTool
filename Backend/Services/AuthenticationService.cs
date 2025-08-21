using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfigurationSection _configuration;
        private readonly UserService _userService;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                RoleManager<IdentityRole> roleManager,
                IConfigurationSection configuration,
                UserService userService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<IdentityResult> RegisterUserAsync(ApplicationUser user, string password, string? role = null)
        {
            if (user == null || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("User and password must be provided.");
            }
            if (await _userManager.FindByEmailAsync(user.Email) != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }
            // Set UserName to Email for consistency and create the user
            user.UserName = user.Email; // Ensure UserName is set to Email for consistency

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // if no role is specified, default to "User"
                if (string.IsNullOrEmpty(role))
                {
                    role = "User";
                }
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return result;
                }
            }
            await CreateOrUpdatePlayer(user);
            return result;
        }

        public async Task UpdateUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            existingUser.Email = user.Email;
            existingUser.UserName = user.UserName;
            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to update user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(long userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email and password must be provided.");
            }

            return await _signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User") // Default to User role if none assigned
        };

            var issuer = _configuration["Issuer"];
            var audience = _configuration["Audience"];
            var secret = _configuration["Secret"];

            if (string.IsNullOrEmpty(issuer))
                throw new InvalidOperationException("JWT Issuer configuration is missing.");
            if (string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT Audience configuration is missing.");
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("JWT Secret configuration is missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); // Use a secure key from config
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Retrieves an <see cref="ApplicationUser"/> by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The <see cref="ApplicationUser"/> if found; otherwise, null.</returns>
        internal async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        internal async Task InsertRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private async Task CreateOrUpdatePlayer(ApplicationUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var player = await _userService.GetUserByEmail(user.Email);
            if (player == null || !player.Id.HasValue)
            {
                player = new UserDto
                {
                    LastName = user.Email,
                    UserName = user.Email,
                    Email = user.Email,
                    HasLogin = true,
                    Role = 0,
                    AppUserId = user.Id.ToString()
                };
                await _userService.CreateUser(player);
            }
            else
            {
                player.AppUserId = user.Id.ToString();
                player.HasLogin = true;
                await _userService.UpdateUser(player.Id.Value, player);
            }
        }

        internal async Task DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var user = await _userManager.FindByEmailAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
