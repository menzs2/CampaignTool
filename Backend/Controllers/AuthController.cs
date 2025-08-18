using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthController : ControllerBase
    {

        private readonly AuthenticationService _authenticationService;

        public AuthController(
            AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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

                    return Ok(new { Token = JwtToken, Message = "Login successful." });
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

    }
}
