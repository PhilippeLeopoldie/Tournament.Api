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
        var query = FindByCondition(tournament => tournament.Id.Equals(id), trackChanges);

        if (includeGames) 
            query = query.Include(tournament => tournament.Games);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TournamentDetail?> GetByTitleAsync(string title, bool trackChanges)
    {
        return await FindByCondition(Tournament => string.Equals(Tournament.Title, title), trackChanges)
            .FirstOrDefaultAsync();
    }
}
