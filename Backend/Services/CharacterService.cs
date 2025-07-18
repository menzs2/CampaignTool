using Backend.Data;
using Shared;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class CharacterService
{
    private readonly CampaignToolContext _context;

    public CharacterService(CampaignToolContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CharacterDto>> GetAllCharacters()
    {
        var entities = await _context.Characters.ToListAsync();
        return entities.ToDto();
    }

    public async Task<CharacterDto?> GetCharacterById(long id)
    {
        var entity = await _context.Characters.FindAsync(id);
        return entity?.ToDto();
    }

    public async Task<IEnumerable<CharacterDto>> GetCharactersByCampaignId(long campaignId)
    {
        var entities = await _context.Characters
            .Where(c => c.CampaignId == campaignId)
            .ToListAsync();
        return entities.ToDto();
    }
    public async Task<CharacterDto> GetCharacterByName(string name)
    {
        var entity = await _context.Characters.FirstOrDefaultAsync(c => c.Name == name);
        return entity?.ToDto();
    }

    public async Task<IEnumerable<CharacterDto>> GetConnectedCharacters(long characterId)
    {
        var connections = await _context.CharCharConnections
            .Where(cc => cc.CharOneId == characterId || cc.CharTwoId == characterId)
            .ToListAsync();

        var connectedCharacterIds = connections.SelectMany(cc => new[] { cc.CharOneId, cc.CharTwoId })
            .Where(id => id != characterId)
            .Distinct();

        var connectedCharacters = await _context.Characters
            .Where(c => connectedCharacterIds.Contains(c.Id))
            .ToListAsync();
       
        return connectedCharacters.ToDto();
    }

    public async Task<bool> CharacterExists(long id)
    {
        return await _context.Characters.AnyAsync(c => c.Id == id);
    }

    public async Task<CharacterDto> AddCharacterAsync(CharacterDto? characterDto)
    {
        if (characterDto == null)
        {
            throw new ArgumentNullException(nameof(characterDto), "Character data is null.");
        }

        var newCharacter = characterDto.ToModel();
        if (newCharacter == null)
        {
            throw new InvalidOperationException("Failed to convert CharacterDto to Character model.");
        }
        _context.Characters.Add(newCharacter);
        await _context.SaveChangesAsync();
        return newCharacter.ToDto();
    }

    public async Task<CharacterDto> UpdateCharacterAsync(CharacterDto? characterDto)
    {
        if (characterDto == null)
        {
            throw new ArgumentNullException(nameof(characterDto), "Character data is null.");
        }

        var existingCharacter = await _context.Characters.FindAsync(characterDto.Id);
        if (existingCharacter == null)
        {
            throw new KeyNotFoundException($"Character with ID {characterDto.Id} not found.");
        }

        existingCharacter.Name = characterDto.Name;
        existingCharacter.Description = characterDto.Description;
        existingCharacter.CampaignId = characterDto.CampaignId;

        _context.Characters.Update(existingCharacter);
        await _context.SaveChangesAsync();
        return existingCharacter.ToDto();
    }

    public async Task DeleteCharacterAsync(long id)
    {
        var character = await _context.Characters.FindAsync(id);
        if (character == null)
        {
            throw new KeyNotFoundException($"Character with ID {id} not found.");
        }

        _context.Characters.Remove(character);
        await _context.SaveChangesAsync();
    }

}
