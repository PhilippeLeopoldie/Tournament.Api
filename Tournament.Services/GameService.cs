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

    public async Task<Game> PutGameAsync(int id, GameUpdateDto dto)
    {
        var game = await _uow.GameRepository.GetByIdAsync(id, trackChanges: true);
        if (game is null) return game;

        var updatedGame = _mapper.Map(dto,game);
        await _uow.CompleteAsync();
        return updatedGame;
    }

    public async Task<GameUpdateDto?> GameToPatchAsync(int id)
    {
        var gameToPatch = await _uow.GameRepository.GetByIdAsync(id,trackChanges: true);

        return gameToPatch is null ? null : _mapper.Map<GameUpdateDto>(gameToPatch);
    }

    public async Task<bool> SavePatchGameAsync(int id, GameUpdateDto dto)
    {
        var gameToPatch = await _uow.GameRepository.GetByIdAsync(id,trackChanges: true);
        if (gameToPatch is null) return false;
        _mapper.Map(dto, gameToPatch);
        await _uow.CompleteAsync();
        return true;
    }

    public async Task<GameDto> PostGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        _uow.GameRepository.Create(game);
        await _uow.CompleteAsync();
        return _mapper.Map<GameDto>(game);
    }

    public async Task<bool> DeleteGameAsync(int id)
    {
        var tournament = await _uow.GameRepository.GetByIdAsync(id,trackChanges: true);
        if (tournament is null) return false;
        _uow.GameRepository.Delete(tournament);
        await _uow.CompleteAsync();
        return true;
    }

    private async Task<Game> GetGameByIdOrThrowExceptionAsync(int id, bool trackChanges)
    {
        var game = await _uow.GameRepository.GetByIdAsync(id, trackChanges);
        if (game is null) throw new GameNotFoundException(id);
        return game;
    }
}
