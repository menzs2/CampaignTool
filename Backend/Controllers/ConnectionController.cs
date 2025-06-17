using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public ConnectionController(CampaignToolContext campaignToolContext)
    {
        _dbContext = campaignToolContext;
    }
    #region Connections
    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Connections.Any()
            ? Ok(_dbContext.Connections.ToDto())
            : NotFound("No connections found.");
    }
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var connection = _dbContext.Connections.Find(id);
        return connection != null
            ? Ok(connection.ToDto())
            : NotFound($"Connection with ID {id} not found.");
    }
    [HttpPost]
    public IActionResult Post([FromBody] ConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Connection data is required.");
        }

        _dbContext.Connections.Add(connection.ToModel());
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = connection.Id }, connection);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ConnectionDto connection)
    {
        if (connection == null || connection.Id != id)
        {
            return BadRequest("Invalid connection data.");
        }

        var existingConnection = _dbContext.Connections.Find(id);
        if (existingConnection == null)
        {
            return NotFound($"Connection with ID {id} not found.");
        }

        existingConnection.ConnectionName = connection.ConnectionName;
        existingConnection.Description = connection.Description;
        existingConnection.GmOnlyDescription = connection.GmOnlyDescription;
        existingConnection.GmOnly = connection.GmOnly;
        existingConnection.CampaignId = connection.CampaignId;
        _dbContext.Connections.Update(existingConnection);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var connection = _dbContext.Connections.Find(id);
        if (connection == null)
        {
            return NotFound($"Connection with ID {id} not found.");
        }

        _dbContext.Connections.Remove(connection);
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpGet("charchar")]
    public IActionResult GetAll()
    {
        return _dbContext.CharCharConnections.Any()
            ? Ok(_dbContext.CharCharConnections.ToDto())
            : NotFound("No connections found.");
    }
    [HttpGet("charchar/{id}")]
    public IActionResult GetCharCharConnection(int id)
    {
        var connection = _dbContext.CharCharConnections.Find(id);
        return connection != null
            ? Ok(connection.ToDto())
            : NotFound($"Character-Character connection with ID {id} not found.");
    }
    [HttpPost("charchar")]
    public IActionResult PostCharCharConnection([FromBody] CharCharConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Character connection data is required.");
        }

        _dbContext.CharCharConnections.Add(connection.ToModel());
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetCharCharConnection), new { id = connection.Id }, connection);
    }
    [HttpPut("charchar/{id}")]
    public IActionResult PutCharCharConnection(int id, [FromBody] CharCharConnectionDto connection)
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

    [HttpDelete("charchar/{id}")]
    public IActionResult DeleteCharCharConnection(int id)
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

    [HttpGet("charorg")]
    public IActionResult GetAllCharOrgConnections()
    {
        return _dbContext.CharOrgConnections.Any()
            ? Ok(_dbContext.CharOrgConnections.ToDto())
            : NotFound("No character-organization connections found.");
    }
    
    [HttpGet("charorg/{id}")]
    public IActionResult GetCharOrgConnection(int id)
    {
        var connection = _dbContext.CharOrgConnections.Find(id);
        return connection != null
            ? Ok(connection.ToDto())
            : NotFound($"Character-Organization connection with ID {id} not found.");
    }

    [HttpPost("charorg")]
    public IActionResult PostCharOrgConnection([FromBody] CharOrgConnectionDto connection)
    {
        if (connection == null)
        {
            return BadRequest("Character-Organization connection data is required.");
        }

        _dbContext.CharOrgConnections.Add(connection.ToModel());
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(GetCharOrgConnection), new { id = connection.Id }, connection);
    }

    [HttpPut("charorg/{id}")]
    public IActionResult PutCharOrgConnection(int id, [FromBody] CharOrgConnectionDto connection)
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

    [HttpDelete("charorg/{id}")]
    public IActionResult DeleteCharOrgConnection(int id)
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
