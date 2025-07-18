using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

//using Tournament.Core.Entities;

namespace Tournament.Core.Contracts;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetGamesAsync(int tournamentId, bool sortedByTitle, bool trackChanges);
    Task<Game> GetByIdAsync(int id, bool trackChanges);
    Task<Game?> GetByTitleAsync(string title, bool trackChanges);
    //Task<bool> AnyAsync(int id);
    void Create(Game game);
    //void Update(Game game);
    void Delete(Game game);
    IQueryable<Game> FindByCondition(Expression<Func<Game, bool>> condition, bool trackChanges = false);
}
