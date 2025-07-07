using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournaments.Shared.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Presentation.Controllers;

[Route("api/Tournaments")]
[ApiController]
public class TournamentsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    

    public TournamentsController (IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    // GET: api/TournamentDetails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAllTournamentAsync(bool includeGames, bool sortByTitle)
    {
        var dto = await _serviceManager.TournamentService.GetAllTournamentsAsync(includeGames,sortByTitle, trackChanges:false);
        return Ok(dto);
    }

    [HttpGet("title")]
    public async Task<ActionResult<TournamentDto>> GetTournamentByTitleAsync(string title)
    {
        var dto = await _serviceManager.TournamentService.GetTournamentByTitleAsync(title);
        if (dto is null) return NotFound($"No tournament with 'Title': {title} found!");
        return Ok(dto);
    }

        // GET: api/TournamentDetails/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournament(int id, bool includeGames)
    {
        var tournamentDto = await _serviceManager.TournamentService
            .GetTournamentByIdAsync(id, includeGames, trackChanges: false);
                                            
        if (tournamentDto is null)
            return NotFound($"No tournament with id:{id} found!");
        
        return Ok(tournamentDto);
    }
    
    // PUT: api/TournamentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest($"id: '{id}' do not match id '{dto.Id}' from body");

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tournamentToUpdate = await _serviceManager.TournamentService.PutTournamentAsync(id , dto);
        
        return tournamentToUpdate is null
            ? NotFound($"No tournament with id: {id} found!")
            : NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTournament(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument)
    {
        if (patchDocument is null) return BadRequest("No patchDocument");

        var tournamentPatchDto = await _serviceManager.TournamentService.TournamentToPatchAsync(id);
        if (tournamentPatchDto is null)
            return NotFound($"No tournament with id: {id} found!");

        patchDocument.ApplyTo(tournamentPatchDto, ModelState);
        TryValidateModel(tournamentPatchDto);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var isSaved = await _serviceManager.TournamentService.SavePatchTournamentAsync(id, tournamentPatchDto);

        return !isSaved 
            ? StatusCode(500, "An error occurred while saving changes!")
            : NoContent();
    }
    
    // POST: api/TournamentDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromBody]TournamentCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdTournament = await _serviceManager.TournamentService.PostTournamentAsync(dto);
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
