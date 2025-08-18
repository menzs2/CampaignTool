using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

public class UserSettingController : ControllerBase
{
    private readonly UserService _service;

    public UserSettingController(UserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Gets user settings.
    /// </summary>
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
        var userSettings = await _service.GetUserSettings();

        return userSettings != null && userSettings.Any() ? Ok(userSettings) : NotFound("No users settings found."); ;
    }

    /// <summary>
    /// Gets a user setting by its ID.
    /// </summary>
    /// <param name="id">The ID of the user setting to retrieve.</param>
    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(long id)
    {
        var userSetting = await _service.GetUserSettingById(id);
        return userSetting != null ? Ok(userSetting) : NotFound($"User setting with ID {id} not found.");
    }

    /// <summary>
    /// Creates a new user setting.
    /// </summary>
    /// <param name="id">
    /// The id of the user
    /// </param>
    /// <param name="userSetting">
    /// The user setting data to create. Can be null; if null, a BadRequest response is returned.
    /// </param>
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post(long id, [FromBody] UserSettingDto? userSetting)
    {
        if (userSetting == null)
        {
            return BadRequest("User setting data is null.");
        }
        try
        {
            var newUserSetting = await _service.CreateUserSetting(id, userSetting);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
        return CreatedAtAction(nameof(Get), new { id = userSetting.Id }, userSetting);
    }

    /// <summary>
    /// Updates an existing user setting.
    /// </summary>
    /// <param name="id">The ID of the user setting to update.</param>
    /// <param name="userSetting">
    /// The user setting data to update. Must not be null and must match the ID.
    /// </param>
    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Put(long id, [FromBody] UserSettingDto? userSetting)
    {
        if (userSetting == null)
        {
            return BadRequest("User setting data is null.");
        }
        try
        {
            await _service.UpdateUserSetting(id, userSetting);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

}
