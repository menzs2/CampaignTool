using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend;

[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly CampaignToolContext _dbContext;

    public CharacterController(CampaignToolContext campaignToolContext)
    {
        _dbContext = campaignToolContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return _dbContext.Characters.Any()
            ? Ok(_dbContext.Characters.ToDto())
            : NotFound("No characters found.");
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var character = _dbContext.Characters.Where(c=> c.Id == id).ToDto();
        return character != null ? Ok(character) : NotFound($"Character with ID {id} not found.");
    }

    [HttpGet("campaign/{campaignId}")]
    public IActionResult GetByCampaign(long campaignId)
    {
        var characters = _dbContext.Characters.Where(c => c.CampaignId == campaignId).ToDto();
        return characters.Any() ? Ok(characters) : NotFound($"No characters found for campaign ID {campaignId}.");
    }

    [HttpGet("connected/{id}")]
    public IActionResult GetConnectedCharacters(long Id)
    {
        var characters = _dbContext.Characters
            .Where(c => c.CharCharConnectionIds.Contains(Id))
            .ToDto();
        return characters != null ? Ok(characters) : NotFound("No connected characters not found.");
    }

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
        return CreatedAtAction(nameof(Get), new { id = character.Id }, character);
    }

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
