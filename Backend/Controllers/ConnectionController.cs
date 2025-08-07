using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    private readonly ConnectionService _service;

    public ConnectionController(ConnectionService connectionService)
    {
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
    /// Gets connections for a campaign ID.
    /// </summary>
    /// <param name="Id">Id of the campaing</param>
    [HttpGet("campaign/{Id}")]
    public async Task<IActionResult> GetConnectionsByCampaignId(long Id)
    {
        var connection = await _service.GetConnectionByCampaignIdAsync(Id);
        return connection != null
            ? Ok(connection)
            : NotFound($"Connection with ID {Id} not found.");
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
            await _service.UpdateConnectionAsync(id, connection);
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

    /// <summary>
    /// Deletes a connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the connection to delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _service.DeleteConnectionAsync(id);
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

    #endregion

    #region Character-Character

    /// <summary>
    /// Gets all character-character connections.
    /// </summary>
    [HttpGet("charchar")]
    public async Task<IActionResult> GetAll()
    {
        var charCharConnections = await _service.GetAllCharToCharConnectionsAsync();
        return charCharConnections is not null && charCharConnections.Any()
            ? Ok(charCharConnections)
            : NotFound("No connections found.");
    }

    /// <summary>
    /// Gets a character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection.</param>
    [HttpGet("charchar/{id}")]
    public async Task<IActionResult> GetCharCharConnection(long id)
    {
        var connection = await _service.GetCharToCharConnectionByIdAsync(id);
        return connection != null
            ? Ok(connection)
            : NotFound($"Character-Character connection with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new character-character connection.
    /// </summary>
    /// <param name="connection">The character-character connection data to add.</param>
    [HttpPost("charchar")]
    public async Task<IActionResult> PostCharCharConnection([FromBody] CharCharConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Character connection data is required.");
        }
        if (connection.CharOneId == connection.CharTwoId)
        {
            return BadRequest("Character-Character connections cannot connect the same character to itself.");
        }

        try
        {
            var entity = await _service.CreateCharToCharConnectionAsync(connection);
            return CreatedAtAction(nameof(GetCharCharConnection), new { id = entity.Id }, connection);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection to update.</param>
    /// <param name="connection">The updated character-character connection data.</param>
    [HttpPut("charchar/{id}")]
    public async Task<IActionResult> PutCharCharConnection(long id, [FromBody] CharCharConnectionDto connection)
    {
        if (connection == null || connection.Id != id)
        {
            return BadRequest("Invalid Character-Character connection data.");
        }
        try
        {
            await _service.UpdateCharToCharConnectionAsync(id, connection);
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

    /// <summary>
    /// Deletes a character-character connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-character connection to delete.</param>
    [HttpDelete("charchar/{id}")]
    public async Task<IActionResult> DeleteCharCharConnection(long id)
    {
        try
        {
            await _service.DeleteCharToCharConnectionAsync(id);
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
    #endregion

    #region Character-Organization

    /// <summary>
    /// Gets all character-organization connections.
    /// </summary>
    [HttpGet("charorg")]
    public async Task<IActionResult> GetAllCharOrgConnections()
    {
        var connections = await _service.GetAllCharToOrgConnectionsAsync();
        return connections is not null && connections.Any()
            ? Ok(connections)
            : NotFound("No character-organization connections found.");
    }

    /// <summary>
    /// Gets a character-organization connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-organization connection.</param>
    [HttpGet("charorg/{id}")]
    public async Task<IActionResult> GetCharOrgConnection(long id)
    {
        var connection = await _service.GetCharToOrgConnectionByIdAsync(id);
        return connection != null
            ? Ok(connection)
            : NotFound($"Character-Organization connection with ID {id} not found.");
    }

    /// <summary>
    /// Adds a new character-organization connection.
    /// </summary>
    /// <param name="connection">The character-organization connection data to add.</param>
    [HttpPost("charorg")]
    public async Task<IActionResult> PostCharOrgConnection([FromBody] CharOrgConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Organization connection data is required.");
        }
        try
        {
            var entity = await _service.CreateCharToOrgConnectionAsync(connection);
            return CreatedAtAction(nameof(GetCharOrgConnection), new { id = entity.Id }, connection);

        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing character-organization connection by its ID.
    /// </summary>
    /// <param name="id">The ID of the character-organization connection to update.</param>
    /// <param name="connection">The updated character-organization connection data.</param>
    [HttpPut("charorg/{id}")]
    public async Task<IActionResult> PutCharOrgConnection(long id, [FromBody] CharOrgConnectionDto connection)
    {
        if (connection == null || connection.Id != id)
        {
            return BadRequest("Invalid Character-Organization connection data.");
        }
        try
        {
            await _service.UpdateCharacterToOrganizationConnectionAsync(1, connection);
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

    /// <summary>
    /// Deletes a character-organization connection by its ID.
    /// /// </summary>
    /// <param name="id">The ID of the character-organization connection to delete.</param>
    [HttpDelete("charorg/{id}")]
    public IActionResult DeleteCharOrgConnection(long id)
    {
        try
        {
            var connection = _service.DeleteCharacterToOrganizationConnectionAsync(id);
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

    #endregion
}
