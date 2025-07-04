using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournaments.Shared.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Presentation.Controllers;

[Route("api/Tournaments")]
[ApiController]
public class TournamentsController : ControllerBase
{
    private readonly IUnitOfWork _uow; 
    private readonly IMapper _mapper;
    private readonly IServiceManager _serviceManager;
    

    public TournamentsController (IUnitOfWork uow, IMapper mapper, IServiceManager serviceManager)
    {
        _uow = uow;
        _mapper = mapper;
        _serviceManager = serviceManager;
    }

    // GET: api/TournamentDetails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAllTournamentAsync(bool includeGames, bool sortByTitle)
    {
        /*var dto = _mapper
            .Map<IEnumerable<TournamentDto>>(await _uow.TournamentRepository
            .GetAllAsync(includeGames, sortByTitle, trackChanges: false));*/
        var dto = await _serviceManager.TournamentService.GetAllTournamentsAsync(includeGames,sortByTitle, trackChanges:false);
        return Ok(dto);
    }
    
    // GET: api/TournamentDetails/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id, bool includeGames)
    {
        var tournamentDto = await _serviceManager.TournamentService
            .GetTournamentAsync(id, includeGames, trackChanges: false);
                                            
        if (tournamentDto == null)
            return NotFound($"No tournament with id:{id} found!");
        
        return Ok(tournamentDto);
    }
    
    // PUT: api/TournamentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var tournamentToUpdate = await _serviceManager.TournamentService.PutTournamentAsync(id , dto);
        if (tournamentToUpdate == null)
             return NotFound($"No tournament with id: {id} found!");
        
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTournament(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument)
    {
        if (patchDocument == null) return BadRequest("No patchDocument");

        var tournamentPatchDto = await _serviceManager.TournamentService.TournamentToPatchAsync(id);
        if (tournamentPatchDto == null)
            return NotFound($"No tournament with id: {id} found!");

        patchDocument.ApplyTo(tournamentPatchDto, ModelState);
        TryValidateModel(tournamentPatchDto);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var isSaved = await _serviceManager.TournamentService.SavePatchTournamentAsync(id, tournamentPatchDto);
        if (!isSaved) return StatusCode(500, "An error occurred while saving changes!");

        return NoContent();
    }
    
    // POST: api/TournamentDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromQuery]TournamentCreateDto dto)
    {
        var createdTournament = await _serviceManager.TournamentService.PostTournamentDetails(dto);
        return CreatedAtAction("GetTournament", new { id = createdTournament.Id }, createdTournament);
    }
    
    // DELETE: api/TournamentDetails/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentAsync(int id)
    {
        var isDeleted= await _serviceManager.TournamentService.DeleteTournamentAsync(id);

        return  !isDeleted 
            ? NotFound($"Tournament with id:{id} not found!")
            : NoContent();
    }
    

}
