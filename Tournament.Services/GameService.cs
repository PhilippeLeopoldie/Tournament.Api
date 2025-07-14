using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Tournaments.Shared.Dtos;

namespace Tournament.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GameService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GameDto>> GetGamesAsync(int tournamentId, bool sortByTitle, bool trackChanges = false)
    {
        return _mapper.Map<IEnumerable<GameDto>>(await _uow.GameRepository.GetGamesAsync(tournamentId,sortByTitle, trackChanges));
    }

    public async Task<GameDto> GetGameByIdAsync(int id, bool trackChanges)
    {
        var game = await GetGameByIdOrThrowExceptionAsync(id, trackChanges:false);
        return _mapper.Map<GameDto>(game);
    }

    public async Task<GameDto> GetGameByTitleAsync(string title)
    {
        var game = await _uow.GameRepository.GetByTitleAsync(title, trackChanges: false);
        if(game is null) throw new GameNotFoundException(title);
        return _mapper.Map<GameDto>(game);
    }

    public async Task PutGameAsync(int id, GameUpdateDto dto)
    {
        if (id != dto.Id) throw new InvalidEntryBadRequestException(id);
        var game = await GetGameByIdOrThrowExceptionAsync(id, trackChanges: true);
        var updatedGame = _mapper.Map(dto,game);
        await _uow.CompleteAsync();
    }

    public async Task<(Game,GameUpdateDto)> GameToPatchAsync(int gameId, int tournamentId)
    {
        var game = await GetGameByIdForTournamentOrThrowExceptionAsync(gameId, tournamentId, trackChanges:true);
        var dto = _mapper.Map<GameUpdateDto>(game);
        return (game,dto);
    }

    public async Task SavePatchGameAsync(Game game, GameUpdateDto dto)
    {
        _mapper.Map(dto, game);
        await _uow.CompleteAsync();
    }

    public async Task<GameDto> PostGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        _uow.GameRepository.Create(game);
        await _uow.CompleteAsync();
        return _mapper.Map<GameDto>(game);
    }

    public async Task DeleteGameAsync(int gameId, int tournamentId)
    {
        var tournament = await GetGameByIdForTournamentOrThrowExceptionAsync(gameId, tournamentId,trackChanges: true);
        _uow.GameRepository.Delete(tournament);
        await _uow.CompleteAsync();
    }

    private async Task<Game> GetGameByIdOrThrowExceptionAsync(int id, bool trackChanges)
    {
        var game = await _uow.GameRepository.GetByIdAsync(id, trackChanges);
        if (game is null) throw new GameNotFoundException(id);
        return game;
    }

    private async Task<Game> GetGameByIdForTournamentOrThrowExceptionAsync(int gameId,int tournamentId, bool trackChanges)
    {
        var game = await _uow.GameRepository.FindByCondition(
            game => game.Id.Equals(gameId) && game.TournamentDetailId.Equals(tournamentId),
            trackChanges).FirstOrDefaultAsync();
        if (game is null) throw new InvalidGameIdForTournamentBadRequestException(gameId, tournamentId);
        return game;
    }
}
