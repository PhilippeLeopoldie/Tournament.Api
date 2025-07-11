using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournaments.Shared.Dtos;

namespace Services.Contracts;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetAllGamesAsync(bool sortByTitle, bool trackChanges);
    Task<GameDto> GetGameByIdAsync(int id, bool trackChanges);
    Task<GameDto> GetGameByTitleAsync(string title);
    Task PutGameAsync(int id, GameUpdateDto dto);
    Task<GameUpdateDto> GameToPatchAsync(int id);
    Task<bool> SavePatchGameAsync(int id, GameUpdateDto dto);
    Task<GameDto> PostGameAsync(GameCreateDto dto);
    Task<bool> DeleteGameAsync(int id);
}
