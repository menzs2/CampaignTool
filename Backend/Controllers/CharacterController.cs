using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers;

/// <summary>
/// 1. Controller for managing characters in the campaign tool.
/// 2. Provides endpoints to retrieve, create, update, and delete characters.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public CharacterController(CampaignToolContext campaignToolContext)
    {
        _dbContext = campaignToolContext;
    }

    /// <summary>
    /// Retrieves all characters.   
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Characters.Any()
            ? Ok(_dbContext.Characters.ToDto())
            : NotFound("No characters found.");
    }

    /// <summary>
    /// Retrieves a character by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var character = _dbContext.Characters.Where(c => c.Id == id).ToDto();
        return character != null ? Ok(character) : NotFound($"Character with ID {id} not found.");
    }

    /// <summary>
    /// Retrieves characters associated with a specific campaign.
    /// </summary>
    [HttpGet("campaign/{campaignId}")]
    public IActionResult GetByCampaign(long campaignId)
    {
        var characters = _dbContext.Characters.Where(c => c.CampaignId == campaignId).ToDto();
        return characters.Any() ? Ok(characters) : NotFound($"No characters found for campaign ID {campaignId}.");
    }

    /// <summary>
    /// Retrieves characters connected to a specific character by ID.
    /// </summary>
    [HttpGet("connected/{id}")]
    public IActionResult GetConnectedCharacters(long Id)
    {
        var characters = _dbContext.Characters
            .Where(c => c.CharCharConnectionIds.Contains(Id))
            .ToDto();
        return characters != null ? Ok(characters) : NotFound("No connected characters found.");
    }

    /// <summary>
    /// Creates a new character.
    /// </summary>
    [HttpPost]
    public IActionResult Post([FromBody] CharacterDto character)
    {
        if (character == null)
        {
            return BadRequest("Character data is null.");
        }
        var newCharacter = character.ToModel();
        if (newCharacter == null)
        {
            return BadRequest("Invalid character data.");
        }
        _dbContext.Characters.Add(newCharacter);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = newCharacter.Id }, newCharacter.ToDto());
    }

    /// <summary>
    /// Updates an existing character.
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] CharacterDto character)
    {
        if (character == null || character.Id != id)
        {
            return BadRequest("Character data is invalid.");
        }

        var existingCharacter = _dbContext.Characters.Find(id);
        if (existingCharacter == null)
        {
            return NotFound($"Character with ID {id} not found.");
        }

        existingCharacter.CharacterName = character.CharacterName;
        existingCharacter.Description = character.Description;
        existingCharacter.CampaignId = character.CampaignId;

        _dbContext.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletes a character by its ID.
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        var character = _dbContext.Characters.Find(id);
        if (character == null)
        {
            return NotFound($"Character with ID {id} not found.");
        }

        _dbContext.Characters.Remove(character);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
