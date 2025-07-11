using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
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

    public async Task<IEnumerable<GameDto>> GetAllGamesAsync(bool sortByTitle, bool trackChanges = false)
    {
        return _mapper.Map<IEnumerable<GameDto>>(await _uow.GameRepository.GetAllAsync(sortByTitle, trackChanges));
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
        if (id != dto.Id) throw new InvalidIdBadRequestException(id);
        var game = await GetGameByIdOrThrowExceptionAsync(id, trackChanges: true);
        var updatedGame = _mapper.Map(dto,game);
        await _uow.CompleteAsync();
    }

    public async Task<(Game,GameUpdateDto)> GameToPatchAsync(int gameId, int tournamentId)
    {
        if(gameId != tournamentId) throw new InvalidIdBadRequestException(gameId);
        var game = await GetGameByIdOrThrowExceptionAsync(gameId, trackChanges: true);
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

    public async Task DeleteGameAsync(int id)
    {
        var tournament = await GetGameByIdOrThrowExceptionAsync(id,trackChanges: true);
        _uow.GameRepository.Delete(tournament);
        await _uow.CompleteAsync();
    }

    private async Task<Game> GetGameByIdOrThrowExceptionAsync(int id, bool trackChanges)
    {
        var game = await _uow.GameRepository.GetByIdAsync(id, trackChanges);
        if (game is null) throw new GameNotFoundException(id);
        return game;
    }
}
