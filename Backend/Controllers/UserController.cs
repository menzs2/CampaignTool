using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public UserController(CampaignToolContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Users.Any()
            ? Ok(_dbContext.Users.ToDto())
            : NotFound("No users found.");
    }

    /// <summary>
    /// Retrieves a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var user = _dbContext.Users.Where(u => u.Id == id).FirstOrDefault()?.ToDto();
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }
        return user != null ? Ok(user) : NotFound($"User with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">
    /// The user data to add. Can be null; if null, a BadRequest response is returned.
    /// </param>
    [HttpPost]
    public IActionResult Post([FromBody] UserDto? user)
    {
        if (user == null)
        {
            return BadRequest("User data is null.");
        }
        var newUser = user.ToModel();
        if (newUser == null)
        {
            return BadRequest("Invalid user data.");
        }

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        // Automatically create a UserSetting for the new user
        var usersetting = new UserSetting
        {
            UserId = newUser.Id,
            DefaultCampaignId = null,
            SelectLastCampaign = true,
            SameNameWarning = true
        };
        _dbContext.UserSettings.Add(usersetting);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, user);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] UserDto? user)
    {
        if (user == null || user.Id != id)
        {
            return BadRequest("User data is invalid.");
        }

        var existingUser = _dbContext.Users.Find(id);
        if (existingUser == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;

        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <remarks>
    /// This action will remove the user from the database.
    /// </remarks>
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var user = _dbContext.Users.Find(id);
        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        _dbContext.Users.Remove(user);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
