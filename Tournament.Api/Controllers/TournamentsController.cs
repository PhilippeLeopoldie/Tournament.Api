using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetail tournamentDetails)
        {
            if (id != tournamentDetails.Id)
            {
                return BadRequest();
            }

            try
            {
                 _uow.TournamentRepository.Update(tournamentDetails);
                 await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _uow.TournamentRepository.AnyAsync(id))
                {
                    return NotFound($"No tournament with id: {id} found!");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetail>> PostTournamentDetails(TournamentDetail tournamentDetails)
        {
            _uow.TournamentRepository.Add(tournamentDetails);
            await _uow.CompleteAsync();

            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDetails);
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
