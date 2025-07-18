using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;

namespace Services.Contracts;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetGamesAsync(int tournamentId, bool sortByTitle, bool trackChanges);
    Task<GameDto> GetGameByIdAsync(int id, bool trackChanges);
    Task<GameDto> GetGameByTitleAsync(string title);
    Task PutGameAsync(int id, GameUpdateDto dto);
    Task<(Game, GameUpdateDto)> GameToPatchAsync(int gameId, int tounamentId);
    Task SavePatchGameAsync(Game game, GameUpdateDto dto);
    Task<GameDto> PostGameAsync(GameCreateDto dto);
    Task DeleteGameAsync(int id, int tournamentId);
}
