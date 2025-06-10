using Backend.Data;
using Microsoft.AspNetCore.Mvc;

namespace Backend;

[Route("api/[controller]")]
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
            ? Ok(_dbContext.Characters.ToList())
            : NotFound("No characters found.");
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var character = _dbContext.Characters.Find(id);
        return character != null ? Ok(character) : NotFound($"Character with ID {id} not found.");
    }

    [HttpPost]
    public IActionResult Post([FromBody] Models.Character character)
    {
        if (character == null)
        {
            return BadRequest("Character data is null.");
        }

        _dbContext.Characters.Add(character);
        _dbContext.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = character.Id }, character);
    }

    [HttpPut("{id}")]
    public IActionResult Put(long id, [FromBody] Models.Character character)
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
