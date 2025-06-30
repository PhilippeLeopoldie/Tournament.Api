using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class TournamentRepository : RepositoryBase<TournamentDetail> ,ITournamentRepository
{
    public TournamentRepository(TournamentApiContext context) : base(context)
    {
    } 

    public async Task<bool>AnyAsync(int id)
    {
        return await AnyAsync(id);
    }

    public async Task<IEnumerable<TournamentDetail>> GetAllAsync(
        bool includeGames = false,
        bool sortByTitle = false,
        bool trackChanges = false
        )
    {
        var query = FindAll(trackChanges);

        if (includeGames)
            query = query.Include(tournament => tournament.Games);

        if (sortByTitle)
            query = query.OrderBy(tournament => tournament.Title);

        return await query.ToListAsync();
    }

    public async Task<TournamentDetail?> GetAsync(
         int id,
         bool includeGames = false,
         bool trackChanges = false)
    {  
        return includeGames ?
            await FindByCondition(tournament => tournament.Id.Equals(id), trackChanges)
            .Include(tournament => tournament.Games)
            .FirstOrDefaultAsync()
            : await FindByCondition(tournament => tournament.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();
    }
}
