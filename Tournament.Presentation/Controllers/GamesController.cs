using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournaments.Shared.Dtos;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager ??
                throw new ArgumentNullException(nameof(serviceManager));
        }

        // GET: api/Games
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync([FromQuery]int tournamentId, bool sortByTitle)
        {
            var dto = await _serviceManager.GameService.GetGamesAsync(tournamentId,sortByTitle, trackChanges: false);
            return Ok(dto);
        }

        // GET: api/Games/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            var gameDto = await _serviceManager.GameService.GetGameByIdAsync(id, trackChanges: false);
            return Ok(gameDto);
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("title")]
        public async Task<ActionResult<GameDto>> GetGameByTitleAsync(string title)
        {
            var dto = await _serviceManager.GameService.GetGameByTitleAsync(title);
            return Ok(dto);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameAsync(int id, GameUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _serviceManager.GameService.PutGameAsync(id, dto);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchGameAsync(int id, int tournamentId, JsonPatchDocument<GameUpdateDto> patchDocument)
        {
            if (patchDocument is null) throw new InvalidEntryBadRequestException();

            var (game,gamePatchDto) = await _serviceManager.GameService.GameToPatchAsync(gameId:id,tournamentId);

            patchDocument.ApplyTo(gamePatchDto, ModelState);
            TryValidateModel(gamePatchDto);
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            await _serviceManager.GameService.SavePatchGameAsync(game, gamePatchDto);
            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGameAsync([FromBody]GameCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdGame = await _serviceManager.GameService.PostGameAsync(dto);
            return CreatedAtAction("GetGameById", new { id = createdGame.Id }, createdGame);
        }

        // DELETE: api/Games/5
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameAsync(int id, int tournamentId)
        {
            await _serviceManager.GameService.DeleteGameAsync(id, tournamentId);
            return NoContent();
        }
    }
}
