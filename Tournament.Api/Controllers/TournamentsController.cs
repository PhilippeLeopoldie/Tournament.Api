using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Tournaments.Shared.Dtos;

namespace Tournament.Api.Controllers
{
    [Route("api/Tournaments")]
    [ApiController]
    public class TournamentsController(IUnitOfWork uow, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            var dto = _mapper
                .Map<IEnumerable<TournamentDto>>(await _uow.TournamentRepository.GetAllAsync());
            return Ok(dto);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentDetails is null)
            {
                return NotFound($"No tournament with id:{id} found!");
            }

            var dto = _mapper.Map<TournamentDto>(tournamentDetails);
            return dto;
        }
        
        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var tournamentExists = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentExists == null)
            {
                 return NotFound($"No tournament with id: {id} found!");
            }

            _mapper.Map(dto, tournamentExists);
            await _uow.CompleteAsync();
            return NoContent();
        }
        
        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromQuery]TournamentCreateDto dto)
        {
            var tournament = _mapper.Map<TournamentDetail>(dto);
            _uow.TournamentRepository.Add(tournament);
            await _uow.CompleteAsync();
            var createdTournament = _mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction("GetTournamentDetails", new { id = createdTournament.Id }, createdTournament);
        }
        
        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound($"Tournament with id:{id} not found!");
            }

            _uow.TournamentRepository.Delete(tournamentDetails);
            await _uow.CompleteAsync();
            return NoContent();
        }
    }
}
