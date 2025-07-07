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
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync(bool sortByTitle)
        {
            var dto = await _serviceManager.GameService.GetAllGamesAsync(sortByTitle, trackChanges: false);
            return Ok(dto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGameByIdAsync(int id)
        {
            var gameDto = await _serviceManager.GameService
            .GetGameByIdAsync(id, trackChanges: false);

            if (gameDto is null)
                return NotFound($"No game with id:{id} found!");

            return Ok(gameDto);
        }

        [HttpGet("title")]
        public async Task<ActionResult<GameDto>> GetGameByTitleAsync(string title)
        {
            var dto = await _serviceManager.GameService.GetGameByTitleAsync(title);
            if (dto is null) return NotFound($"No game with 'Title': {title} found!");
            return Ok(dto);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameAsync(int id, GameUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest($"id: '{id}' do not match id '{dto.Id}' from body");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameToUpdate = await _serviceManager.GameService.PutGameAsync(id, dto);

            return gameToUpdate is null
                ? NotFound($"No tournament with id: {id} found!")
                : NoContent();
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchGameAsync(int id, int tournamentId, JsonPatchDocument<GameUpdateDto> patchDocument)
        {
            if (patchDocument is null) return BadRequest("No patchDocument");

            var gamePatchDto = await _serviceManager.GameService.GameToPatchAsync(id);
            if (gamePatchDto is null)
                return NotFound($"No game with id: {id} found!");

            patchDocument.ApplyTo(gamePatchDto, ModelState);
            TryValidateModel(gamePatchDto);
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            var isSaved = await _serviceManager.GameService.SavePatchGameAsync(id, gamePatchDto);

            return !isSaved
                ? StatusCode(500, "An error occurred while saving changes!")
                : NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGameAsync([FromQuery]GameCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdGame = await _serviceManager.GameService.PostGameAsync(dto);
            return CreatedAtAction("GetGameByIdAsync", new { id = createdGame.Id }, createdGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            var isDeleted = await _serviceManager.GameService.DeleteGameAsync(id);

            return !isDeleted
                ? NotFound($"Game with id:{id} not found!")
                : NoContent();
        }
    }
}
