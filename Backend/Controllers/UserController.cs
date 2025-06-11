using Backend.Data;
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

    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Users.Any()
            ? Ok(_dbContext.Users.ToDto())
            : NotFound("No users found.");
    }

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
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

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
