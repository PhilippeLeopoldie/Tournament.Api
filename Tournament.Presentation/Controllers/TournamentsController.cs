using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Tournament.Core.Exceptions;
using Tournament.Core.Dtos;
using Tournament.Core.Request;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentsAsync([FromQuery]TournamentRequestParams requestParams, bool sortByTitle)
    {
        var pagedResult = await _serviceManager.TournamentService.GetAllTournamentsAsync(requestParams,sortByTitle, trackChanges:false);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
        return Ok(pagedResult.tournamentsDto);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("title")]
    public async Task<ActionResult<TournamentDto>> GetTournamentByTitleAsync(string title)
    {
        return Ok(await _serviceManager.TournamentService.GetTournamentByTitleAsync(title));
    }

    // GET: api/Tournaments/5
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentById(int id,[FromQuery] bool includeGames)
    {
        return Ok(await _serviceManager.TournamentService
            .GetTournamentByIdAsync(id, includeGames)
            );
    }

    // PUT: api/Tournaments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentAsync(int id,[FromBody] TournamentUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _serviceManager.TournamentService.PutTournamentAsync(id , dto);
        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<ActionResult<TournamentDto>> PostTournament([FromBody]TournamentCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdTournament = await _serviceManager.TournamentService.PostTournamentAsync(dto);
        return CreatedAtAction("GetTournamentById", new { id = createdTournament.Id }, createdTournament);
    }

    // DELETE: api/Tournaments/5
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentAsync(int id)
    {
        await _serviceManager.TournamentService.DeleteTournamentAsync(id);
        return NoContent();
    }
    

}
