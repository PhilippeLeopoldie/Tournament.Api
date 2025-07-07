using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public async Task<IEnumerable<GameDto>> GetAllTournamentsAsync(bool sortByTitle, bool trackChanges = false)
    {
        return _mapper.Map<IEnumerable<GameDto>>(await _uow.GameRepository.GetAllAsync(sortByTitle, trackChanges));
    }

    public async Task<GameDto> GetGameByIdAsync(int id, bool trackChanges)
    {
        return _mapper.Map<GameDto>(
            await _uow.GameRepository.GetByIdAsync(id, trackChanges: false)
            );
    }

    public async Task<GameDto> GetGameByTitleAsync(string title)
    {
        var game = await _uow.GameRepository.GetByTitleAsync(title, trackChanges: false);
        return _mapper.Map<GameDto>(game);
    }

    public async Task<Game> PutGameAsync(int id, GameUpdateDto dto)
    {
        var updatedGame = _mapper.Map(dto,
            await _uow.GameRepository.GetByIdAsync(id, trackChanges: true)
            );
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
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteGameAsync(int id)
    {
        throw new NotImplementedException();
    }


}
