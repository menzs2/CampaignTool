using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Backend.Controllers
{
    /// <summary>
    /// 1. Controller for managing characters in the campaign tool.
    /// 2. Provides endpoints to retrieve, create, update, and delete characters.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterService _service;

        public CharacterController(CharacterService characterService)
        {
            _service = characterService;
        }

        /// <summary>
        /// Retrieves all characters.   
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var characters = await _service.GetAllCharacters();
                return characters.Any() ? Ok(characters) : Problem(statusCode: 404, detail: "No characters found.");
            }
            catch (Exception ex)
            {
                // TODO: Log the exception (ex) here
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a character by its ID.
        /// </summary>
        /// <param name="id">The ID of the character to retrieve.</param>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var character = await _service.GetCharacterById(id);
                return character == null ? Problem(statusCode: 404, detail: $"Character with ID {id} not found.") : Ok(character);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves characters associated with a specific campaign.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign to retrieve characters for.</param>
        [HttpGet("campaign/{campaignId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByCampaign(long campaignId)
        {
            try
            {
                var characters = await _service.GetCharactersByCampaignId(campaignId);
                return characters != null && characters.Any() ? Ok(characters) : Problem(statusCode: 404, detail: $"No characters found for campaign ID {campaignId}.");
            }
            catch (Exception ex)
            {
                // TODO: Log the exception (ex) here
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves characters connected to a specific character by ID.
        /// </summary>
        /// <param name="id">The ID of the character to find connections for.</param>
        [HttpGet("connected/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConnectedCharacters(long id)
        {
            var characters = await _service.GetConnectedCharacters(id);
            return characters != null ? Ok(characters) : Problem(statusCode: 404, detail: "No connected characters found.");
        }

        /// <summary>
        /// Creates a new character.
        /// </summary>
        /// <param name="character">The character data to create.</param>  
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] CharacterDto character)
        {
            try
            {
                var newCharacter = await _service.AddCharacterAsync(character);
                return CreatedAtAction(nameof(Get), new { id = newCharacter?.Id }, newCharacter);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception (ex) here
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing character.
        /// </summary>
        /// <param name="id">The ID of the character to update.</param>
        /// <param name="character">The updated character data.</param>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(long id, [FromBody] CharacterDto character)
        {
            try
            {
                await _service.UpdateCharacterAsync(character);
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
        /// Deletes a character by its ID.
        /// </summary>
        /// <param name="id">The ID of the character to delete.</param>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _service.DeleteCharacterAsync(id);
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
    }
}
