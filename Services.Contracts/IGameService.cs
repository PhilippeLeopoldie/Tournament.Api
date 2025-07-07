using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournaments.Shared.Dtos;

namespace Services.Contracts;

public interface IGameService
{
    Task<IEnumerable<GameDto>> GetGameByTitleAsync(bool sortedByTitle);
    Task<GameDto> GetGameByIdAsync(int id, bool trackChanges);

}
