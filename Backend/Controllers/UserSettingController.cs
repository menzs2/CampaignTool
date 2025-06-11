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
