using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournaments.Shared.Dtos;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesAsync(bool sortByTitle)
        {
            var dto = _mapper.Map<IEnumerable<GameDto>>( await _uow.GameRepository.GetAllAsync(sortByTitle));

            return Ok(dto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGameByIdAsync(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id, trackChanges: false);

            if (game == null) 
                return NotFound($"No game with id: '{id}' found!");
            var dto = _mapper.Map<GameDto>(game);
            return dto;
        }

        [HttpGet("title")]
        public async Task<ActionResult<GameDto>> GetGameByTitleAsync(string title)
        {
            var game = await _uow.GameRepository.GetAsync(title, trackChanges: false);

            if (game == null)
                return NotFound($"No game with title: '{title}' found!");
            var dto = _mapper.Map<GameDto>(game);
            return dto;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameAsync(int id, GameUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var GameToUpdate = await _uow.GameRepository.GetAsync(id, trackChanges: true);
            if (GameToUpdate == null)
                return NotFound($"No game with id: '{id}' found!");

            _mapper.Map(dto, GameToUpdate);
            await _uow.CompleteAsync();

            return NoContent();
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchGameAsync(int id, int tournamentId, JsonPatchDocument<GameUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest("No Patch document");
            
            var gameToPatch = await _uow.GameRepository.GetAsync(id, trackChanges: true);

            if (gameToPatch == null) 
                return NotFound($"No game with id: {id} found!");

            if (!gameToPatch.TournamentDetailId.Equals(tournamentId))
                return NotFound($"Game with ID {id} is not associated with Tournament ID {tournamentId}.");

            var dto = _mapper.Map<GameUpdateDto>(gameToPatch);

            //patchDoc.ApplyTo(dto, ModelState);
            TryValidateModel(dto);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(dto, gameToPatch);
            await _uow.CompleteAsync();
            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGameAsync([FromQuery]GameCreateDto dto)
        {
            var game = _mapper.Map<Game>(dto);
            _uow.GameRepository.Create(game);
            await _uow.CompleteAsync();
            var createdGame = _mapper.Map<GameDto>(game);
            return CreatedAtAction("GetGame", new { id = createdGame.Id }, createdGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id, trackChanges: true);
            if (game == null)
                return NotFound($"Game with id:{id} not found!");

            _uow.GameRepository.Delete(game);
            await _uow.CompleteAsync();
            return NoContent();
        }
    }
}
