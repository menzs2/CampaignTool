using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;
    private readonly ConnectionService _service;

    public ConnectionController(CampaignToolContext campaignToolContext, ConnectionService connectionService)
    {
        _dbContext = campaignToolContext;
        _service = connectionService;
    }

    #region Connections

    /// <summary>
    /// Gets all connections.
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return _service.GetConnectionsAsync().Result is { } connections
            ? Ok(connections)
            : NotFound("No connections found.");
    }

    /// <summary>
    /// Gets a connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the connection.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var connection = await _service.GetConnectionByIdAsync(id);
        return connection != null
            ? Ok(connection)
            : NotFound($"Connection with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new connection.
    /// </summary>
    /// <param name="connection">The connection data to add.</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ConnectionDto connection)
    {
        try
        {
            var entity = await _service.CreateConnectionAsync(connection);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, connection);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating connection: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing connection.
    /// </summary>
    /// <param name="id">The ID of the connection to update.</param>
    /// <param name="connection">The updated connection data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] ConnectionDto connection)
    {
        try
        {
            await _service.UpdateConnectionAsync(id,connection);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating connection: {ex.Message}");
        }
        return NoContent();
    }

    /// <summary>
    /// Deletes a connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the connection to delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _service.DeleteConnectionAsync(id);
        return result ? NoContent() : BadRequest($"Connection with ID {id} not found.");
    }

    #endregion

    #region Character-Character

    /// <summary>
    /// Gets all character-character connections.
    /// </summary>
    [HttpGet("charchar")]
    public IActionResult GetAll()
    {
        return _dbContext.CharCharConnections.Any()
            ? Ok(_dbContext.CharCharConnections.Include(ch => ch.CharOne).Include(ch => ch.CharTwo).ToDto())
            : NotFound("No connections found.");
    }

    /// <summary>
    /// Gets a character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection.</param>
    [HttpGet("charchar/{id}")]
    public IActionResult GetCharCharConnection(long id)
    {
        var connection = _dbContext.CharCharConnections.Find(id);
        return connection != null
            ? Ok(connection.ToDto())
            : NotFound($"Character-Character connection with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new character-character connection.
    /// </summary>
    /// <param name="connection">The character-character connection data to add.</param>
    [HttpPost("charchar")]
    public IActionResult PostCharCharConnection([FromBody] CharCharConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Character connection data is required.");
        }
        if (connection.CharOneId == connection.CharTwoId)
        {
            return BadRequest("Character-Character connections cannot connect the same character to itself.");
        }
        var model = connection.ToModel();
        if (model == null)
        {
            return BadRequest("Invalid Character-Character connection data.");
        }
        var entity = _dbContext.CharCharConnections.Add(model);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetCharCharConnection), new { id = entity.Entity.Id }, connection);
    }

    /// <summary>
    /// Updates an existing character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection to update.</param>
    /// <param name="connection">The updated character-character connection data.</param>
    [HttpPut("charchar/{id}")]
    public IActionResult PutCharCharConnection(long id, [FromBody] CharCharConnectionDto connection)
    {
        if (connection == null || connection.Id != id)
        {
            return BadRequest("Invalid Character-Character connection data.");
        }

        var existingConnection = _dbContext.CharCharConnections.Find(id);
        if (existingConnection == null)
        {
            return NotFound($"Character-Character connection with ID {id} not found.");
        }
        existingConnection.Description = connection.Description;
        existingConnection.GmOnlyDescription = connection.GmOnlyDescription;
        existingConnection.GmOnly = connection.GmOnly;
        existingConnection.Direction = connection.Direction;
        existingConnection.CharOneId = connection.CharOneId;
        existingConnection.CharTwoId = connection.CharTwoId;
        existingConnection.ConnectionId = connection.ConnectionId;

        _dbContext.CharCharConnections.Update(existingConnection);
        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes a character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection to delete.</param>
    [HttpDelete("charchar/{id}")]
    public IActionResult DeleteCharCharConnection(long id)
    {
        var connection = _dbContext.CharCharConnections.Find(id);
        if (connection == null)
        {
            return NotFound($"Character-Character connection with ID {id} not found.");
        }

        _dbContext.CharCharConnections.Remove(connection);
        _dbContext.SaveChanges();
        return NoContent();
    }
    #endregion

    #region Character-Organization

    /// <summary>
    /// Gets all character-organization connections.
    /// </summary>
    [HttpGet("charorg")]
    public IActionResult GetAllCharOrgConnections()
    {
        return _dbContext.CharOrgConnections.Any()
            ? Ok(_dbContext.CharOrgConnections.ToDto())
            : NotFound("No character-organization connections found.");
    }

    /// <summary>
    /// Gets a character-organization connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-organization connection.</param>
    [HttpGet("charorg/{id}")]
    public IActionResult GetCharOrgConnection(long id)
    {
        var connection = _dbContext.CharOrgConnections.Find(id);
        return connection != null
            ? Ok(connection.ToDto())
            : NotFound($"Character-Organization connection with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new character-organization connection.
    /// </summary>
    /// <param name="connection">The character-organization connection data to add.</param>
    [HttpPost("charorg")]
    public IActionResult PostCharOrgConnection([FromBody] CharOrgConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Organization connection data is required.");
        }
        var model = connection.ToModel();
        if (model == null)
        {
            return BadRequest("Invalid Character-Organization connection data.");
        }
        var entity = _dbContext.CharOrgConnections.Add(model).Entity;
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetCharOrgConnection), new { id = entity.Id }, connection);
    }

    /// <summary>
    /// Updates an existing character-organization connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-organization connection to update.</param>
    /// <param name="connection">The updated character-organization connection data.</param>
    [HttpPut("charorg/{id}")]
    public IActionResult PutCharOrgConnection(long id, [FromBody] CharOrgConnectionDto connection)
    {
        if (connection == null || connection.Id != id)
        {
            return BadRequest("Invalid Character-Organization connection data.");
        }

        var existingConnection = _dbContext.CharOrgConnections.Find(id);
        if (existingConnection == null)
        {
            return NotFound($"Character-Organization connection with ID {id} not found.");
        }
        existingConnection.Description = connection.Description;
        existingConnection.GmOnlyDescription = connection.GmOnlyDescription;
        existingConnection.GmOnly = connection.GmOnly;
        existingConnection.Direction = connection.Direction;
        existingConnection.CharId = connection.CharId;
        existingConnection.OrganisationId = connection.OrganisationId;
        existingConnection.ConnectionId = connection.ConnectionId;

        _dbContext.CharOrgConnections.Update(existingConnection);
        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes a character-organization connection by its ID.
    /// /// </summary>
    /// <param name="id">The ID of the character-organization connection to delete.</param>
    [HttpDelete("charorg/{id}")]
    public IActionResult DeleteCharOrgConnection(long id)
    {
        var connection = _dbContext.CharOrgConnections.Find(id);
        if (connection == null)
        {
            return NotFound($"Character-Organization connection with ID {id} not found.");
        }

        _dbContext.CharOrgConnections.Remove(connection);
        _dbContext.SaveChanges();
        return NoContent();
    }

    #endregion
}
