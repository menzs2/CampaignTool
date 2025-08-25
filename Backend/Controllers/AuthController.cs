using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthController : ControllerBase
    {

        private readonly AuthenticationService _authenticationService;
        private readonly UserService _userService;

        public AuthController(
            AuthenticationService authenticationService,
            UserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        /// <summary>
        /// Logs in a user with email and password.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid login request.");
            }

            if (ModelState.IsValid)
            {
                var result = await _authenticationService.LoginAsync(request.Email, request.Password);
                if (result.Succeeded)
                {
                    // Issue JWT token
                    var user = await _authenticationService.GetUserByEmailAsync(request.Email);
                    if (user == null)
                    {
                        return BadRequest("User not found.");
                    }
                    var JwtToken = await _authenticationService.GenerateJwtTokenAsync(user);
                    var player = await _userService.GetUserByID(8);
                    return Ok(new LoginResponseDto { Token = JwtToken, User = player, Message = "Login successful." });
                }

                return BadRequest("Invalid username or password.");
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Registers a new user with email, password, and optional role.
        /// </summary>  
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestWithRoleDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid registration request.");
            }
            ApplicationUser user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _authenticationService.RegisterUserAsync(user, request.Password, request.Role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(Register), new { Email = request.Email }, new { Message = "User registered successfully." });
        }

        [HttpDelete("delete")]
        public async Task DeleteUser([FromBody] string email)
        {

            try
            {
                await _authenticationService.DeleteUserAsync(email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete user: " + ex.Message);
            }
        }
    }
}
