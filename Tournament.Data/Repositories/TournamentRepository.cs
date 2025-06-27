using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class TournamentRepository : RepositoryBase<TournamentDetail> ,ITournamentRepository
{
    public TournamentRepository(TournamentApiContext context) : base(context)
    {
        
    }
    public void Create(TournamentDetail tournamentDetails)
    {
        Create(tournamentDetails);
    }

    public async Task<bool>AnyAsync(int id)
    {
        return await AnyAsync(id);
    }

    public async Task<IEnumerable<TournamentDetail>> GetAllAsync(
        bool includeGames = false,
        bool trackChanges = false
        )
    {
        return includeGames ? await FindAll(trackChanges).Include(tournament => tournament.Games).ToListAsync()
                            : await FindAll(trackChanges).ToListAsync();
    }

    public async Task<TournamentDetail> GetAsync(
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

    /*public void Delete(TournamentDetail tournamentDetails)
    {
        context.TournamentDetails.Remove(tournamentDetails);
    }*/

    /*public async void Update(TournamentDetail tournamentDetails)
    {
        context.Entry(tournamentDetails).State = EntityState.Modified;
        
    }*/
}
