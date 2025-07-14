using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class GameRepository : RepositoryBase<Game>, IGameRepository
{
    public GameRepository(TournamentApiContext context) : base(context)
    {
    }


    public async Task<IEnumerable<Game>> GetGamesAsync(int tournamentId,bool sortByTitle = false, bool trackChanges = false)
    {
        var query = FindByCondition(game => game.TournamentDetailId.Equals(tournamentId),trackChanges);
        if (sortByTitle) query = query.OrderBy(game => game.Title);
        return await query.ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id, bool trackChanges = false)
    {
       return await FindByCondition(game => game.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();
    }

    public async Task<Game?> GetByTitleAsync(string title, bool trackChanges = false)
    {
        return await FindByCondition(game => game.Title.ToLower() == title.ToLower() , trackChanges)
            .FirstOrDefaultAsync();
    }

}
