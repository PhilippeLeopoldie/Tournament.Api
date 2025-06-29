using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Infrastructure.Data;
using Tournaments.Shared.Dtos;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly TournamentApiContext _context;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GamesController(TournamentApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var dto = _mapper.Map<IEnumerable<GameDto>>( await _uow.GameRepository.GetAllAsync());

            return Ok(dto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id, trackChanges: false);

            if (game == null) 
                return NotFound($"No game with id: '{id}' found!");
            var dto = _mapper.Map<GameDto>(game);
            return dto;
        }

        [HttpGet("title")]
        public async Task<ActionResult<GameDto>> GetGame(string title)
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
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchGame(int id, int tournamentId, JsonPatchDocument<GameUpdateDto> patchDoc)
        {
            if (patchDoc == null) return BadRequest("No Patch document");
            
            var gameToPatch = await _uow.GameRepository.GetAsync(id, trackChanges: true);

            if (gameToPatch == null) 
                return NotFound($"No game with id: {id} found!");

            if (!gameToPatch.TournamentDetailId.Equals(tournamentId))
                return NotFound($"Game with ID {id} is not associated with Tournament ID {tournamentId}.");

            var dto = _mapper.Map<GameUpdateDto>(gameToPatch);

            patchDoc.ApplyTo(dto, ModelState);
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
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
