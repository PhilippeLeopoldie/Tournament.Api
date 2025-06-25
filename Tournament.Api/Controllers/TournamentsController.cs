using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController(IUnitOfWork unitOfWork) : ControllerBase
    {

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetail>>> GetTournamentDetails()
        {
            return Ok(await unitOfWork.TournamentRepository.GetAllAsync());
            
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetail>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await unitOfWork.TournamentRepository.GetAsync(id);

            if (tournamentDetails is null)
            {
                return NotFound($"No tournament with id:{id} found!");
            }

            return tournamentDetails;
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
                 unitOfWork.TournamentRepository.Update(tournamentDetails);
                 await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await unitOfWork.TournamentRepository.AnyAsync(id))
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
            unitOfWork.TournamentRepository.Add(tournamentDetails);
            await unitOfWork.CompleteAsync();

            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDetails);
        }
        
        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound($"Tournament with id:{id} not found!");
            }

            unitOfWork.TournamentRepository.Delete(tournamentDetails);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
