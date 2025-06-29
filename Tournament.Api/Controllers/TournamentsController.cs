using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournaments.Shared.Dtos;

namespace Tournament.Api.Controllers;

[Route("api/Tournaments")]
[ApiController]
public class TournamentsController : ControllerBase
{
    private readonly IUnitOfWork _uow; 
    private readonly IMapper _mapper; 
    

    public TournamentsController (IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    // GET: api/TournamentDetails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(bool includeGames)
    {
        var dto = _mapper
            .Map<IEnumerable<TournamentDto>>(await _uow.TournamentRepository
            .GetAllAsync(includeGames));

        return Ok(dto);
    }

    // GET: api/TournamentDetails/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, bool includeGames)
    {
        var tournamentDetails = await _uow.TournamentRepository.GetAsync(id, includeGames);
                                            
        if (tournamentDetails is null)
            return NotFound($"No tournament with id:{id} found!");

        var dto = _mapper.Map<TournamentDto>(tournamentDetails);
        return dto;
    }
    
    // PUT: api/TournamentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var tournamentToUpdate = await _uow.TournamentRepository.GetAsync(
            id,
            includeGames:false,
            trackChanges: true
            );
        if (tournamentToUpdate == null)
             return NotFound($"No tournament with id: {id} found!");
        
        _mapper.Map(dto, tournamentToUpdate);
        await _uow.CompleteAsync();
        
        return NoContent();
    }

    [HttpPatch("{tournamentId}")]
    public async Task<ActionResult> PatchTournament(int tournamentId, JsonPatchDocument<TournamentUpdateDto> patchDocument)
    {
        if (patchDocument == null) return BadRequest("No patchDocument");

        var tournamentToPatch = await _uow.TournamentRepository.GetAsync(
            tournamentId,
            includeGames: true,
            trackChanges: true
            );
        if (tournamentToPatch == null)
            return NotFound($"No tournament with id: {tournamentId} found!");

        var dto = _mapper.Map<TournamentUpdateDto>(tournamentToPatch);
        patchDocument.ApplyTo(dto, ModelState);
        TryValidateModel(dto);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        _mapper.Map(dto, tournamentToPatch);
        await _uow.CompleteAsync();
        return NoContent();
    }
    
    // POST: api/TournamentDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromQuery]TournamentCreateDto dto)
    {
        var tournament = _mapper.Map<TournamentDetail>(dto);
        _uow.TournamentRepository.Create(tournament);
        await _uow.CompleteAsync();
        var createdTournament = _mapper.Map<TournamentDto>(tournament);
        return CreatedAtAction("GetTournamentDetails", new { id = createdTournament.Id }, createdTournament);
    }
    
    // DELETE: api/TournamentDetails/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var tournamentDetails = await _uow.TournamentRepository.GetAsync(id, true);
        if (tournamentDetails == null)
            return NotFound($"Tournament with id:{id} not found!");

        _uow.TournamentRepository.Delete(tournamentDetails);
        await _uow.CompleteAsync();
        return NoContent();
    }


}
