using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly CampaignService _campaignService;

        public CampaignController(CampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        /// <summary>
        /// Gets all campaigns.
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get()
        {
            var campaigns = await _campaignService.GetAllCampaigns();
            return campaigns.Any() ? Ok(campaigns) : NotFound("No campaigns found.");
        }

        /// <summary>
        /// Gets a campaign by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(long id)
        {
            var campaign = await _campaignService.GetCampaignById(id);
            return campaign != null ? Ok(campaign) : Problem(detail: $"Campaign with ID {id} not found.", statusCode: 404);
        }

        /// <summary>
        /// Gets campaigns associated with a specific player by their ID. As a GM or player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("player/{playerId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetByPlayerId(long playerId)
        {
            var campaigns = await _campaignService.GetCampaignsByUserId(playerId);
            return campaigns.Any() ? Ok(campaigns) : Problem($"No campaigns found for player with ID {playerId}.", statusCode: 404);
        }

        /// <summary>
        /// Adds a new campaign.
        /// </summary>
        /// <param name="campaign">The campaign data to add.</param>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] CampaignDto campaign)
        {
            try
            {
                var newCampaign = await _campaignService.AddCampaignAsync(campaign);
                return CreatedAtAction(nameof(Get), new { id = newCampaign?.Id }, newCampaign);
            }
            catch (Exception ex)
            {
                // TODO: Log the exception.
                return Problem("Internal server error", statusCode: 500);
            }
        }

        /// <summary>
        /// Updates an existing campaign by its ID.
        /// </summary>
        /// <param name="id">The ID of the campaign to update.</param>
        /// <param name="campaign">The updated campaign data.</param>
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put(long id, [FromBody] CampaignDto campaign)
        {
            try
            {
                var updatedCampaign = await _campaignService.UpdateCampaignAsync(campaign);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: 404);
            }
            catch
            {
                // TODO: Log the exception.
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a campaign by its ID.
        /// </summary>
        /// <param name="id">The ID of the campaign to delete.</param>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _campaignService.DeleteCampaignAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: 404);
            }
            catch
            {
                // TODO: Log the exception.
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
