using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

public class UserSettingController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public UserSettingController(CampaignToolContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets user settings.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        var userSetting = _dbContext.UserSettings.FirstOrDefault()?.ToDto();
        if (userSetting == null)
        {
            return NotFound("User settings not found.");
        }
        return Ok(userSetting);
    }

    /// <summary>
    /// Gets a user setting by its ID.
    /// </summary>
    /// <param name="id">The ID of the user setting to retrieve.</param>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var userSetting = _dbContext.UserSettings.Where(us => us.Id == id).FirstOrDefault()?.ToDto();
        if (userSetting == null)
        {
            return NotFound($"User setting with ID {id} not found.");
        }
        return Ok(userSetting);
    }

    /// <summary>
    /// Creates a new user setting.
    /// </summary>
    /// <param name="userSetting">
    /// The user setting data to create. Can be null; if null, a BadRequest response is returned.
    /// </param>
    [HttpPost]
    public IActionResult Post([FromBody] UserSettingDto? userSetting)
    {
        if (userSetting == null)
        {
            return BadRequest("User setting data is null.");
        }
        var newUserSetting = userSetting.ToModel();
        if (newUserSetting == null)
        {
            return BadRequest("Invalid user setting data.");
        }

        _dbContext.UserSettings.Add(newUserSetting);
        _dbContext.SaveChanges();
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
    public IActionResult Put(long id, [FromBody] UserSettingDto? userSetting)
    {
        if (userSetting == null || userSetting.Id != id)
        {
            return BadRequest("User setting data is invalid.");
        }

        var existingUserSetting = _dbContext.UserSettings.Find(id);
        if (existingUserSetting == null)
        {
            return NotFound($"User setting with ID {id} not found.");
        }

        existingUserSetting.SelectLastCampaign = userSetting.SelectLastCampaign;
        existingUserSetting.SameNameWarning = userSetting.SameNameWarning;
        existingUserSetting.DefaultCampaignId = userSetting.DefaultCampaignId;

        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes a user setting by its ID.
    /// </summary>
    /// <param name="id">The ID of the user setting to delete.</param>
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var userSetting = _dbContext.UserSettings.Find(id);
        if (userSetting == null)
        {
            return NotFound($"User setting with ID {id} not found.");
        }

        _dbContext.UserSettings.Remove(userSetting);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
