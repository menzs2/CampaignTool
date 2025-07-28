using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _service.GetAllUsers();
        return users.Any()
            ? Ok(users)
            : NotFound("No users found.");
    }

    /// <summary>
    /// Retrieves a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var user = await _service.GetUserByID(id);
        return user != null ? Ok(user) : NotFound($"User with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">
    /// The user data to add. Can be null; if null, a BadRequest response is returned.
    /// </param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserDto? user)
    {
        if (user == null)
        {
            return BadRequest("User data is null.");
        }
        try
        {
            var newUser = await _service.CreateUser(user);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, user);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] UserDto? user)
    {
        if (user == null || user.Id != id)
        {
            return BadRequest("User data is invalid.");
        }

        try
        {
            await _service.UpdateUser(id, user);
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

    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <remarks>
    /// This action will remove the user from the database.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _service.DeleteUser(id);
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
        return NoContent();
    }
}
