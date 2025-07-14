using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournaments.Shared.Dtos;
using Microsoft.AspNetCore.JsonPatch;
using Tournaments.Shared.Request;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Domain.Models.Exceptions;

namespace Tournament.Presentation.Controllers;

[Route("api/Tournaments")]
[ApiController]
public class TournamentsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    

    public TournamentsController (IServiceManager serviceManager)
    {
        _serviceManager = serviceManager ??
            throw new ArgumentNullException(nameof(serviceManager));
    }

    // GET: api/Tournaments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentsAsync([FromQuery]TournamentRequestParams requestParams, bool sortByTitle)
    {
        var pagedResult = await _serviceManager.TournamentService.GetAllTournamentsAsync(requestParams,sortByTitle, trackChanges:false);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.tournamentsDto);
    }

    [HttpGet("title")]
    public async Task<ActionResult<TournamentDto>> GetTournamentByTitleAsync(string title)
    {
        return Ok(await _serviceManager.TournamentService.GetTournamentByTitleAsync(title));
    }

        // GET: api/Tournaments/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentById(int id,[FromQuery] bool includeGames)
    {
        return Ok(await _serviceManager.TournamentService
            .GetTournamentByIdAsync(id, includeGames, trackChanges: false)
            );
    }
    
    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentAsync(int id,[FromBody] TournamentUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _serviceManager.TournamentService.PutTournamentAsync(id , dto);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTournamentAsync(int id,[FromBody] JsonPatchDocument<TournamentUpdateDto> patchDocument)
    {
        if (patchDocument is null) throw new InvalidEntryBadRequestException();

        var (tournament, patchDto) = await _serviceManager.TournamentService.TournamentToPatchAsync(id);

        patchDocument.ApplyTo(patchDto, ModelState);
        TryValidateModel(patchDto);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        await _serviceManager.TournamentService.SavePatchTournamentAsync(tournament ,patchDto);
        return NoContent();
    }
    
    // POST: api/Tournaments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournamentDetails([FromBody]TournamentCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdTournament = await _serviceManager.TournamentService.PostTournamentAsync(dto);
        return CreatedAtAction("GetTournamentById", new { id = createdTournament.Id }, createdTournament);
    }
    
    // DELETE: api/Tournaments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentAsync(int id)
    {
        await _serviceManager.TournamentService.DeleteTournamentAsync(id);
        return NoContent();
    }
    

}
