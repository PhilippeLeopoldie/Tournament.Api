using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core;
using Tournament.Core.Repositories;

namespace Tournament.Data.Data.Repositories;

public class TournamentRepository(TournamentApiContext context) : ITournamentRepository
{
    public void Add(TournamentDetails tournamentDetails)
    {
        throw new NotImplementedException();
    }

    public Task<bool>AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
    {
        return await context.TournamentDetails.ToListAsync();
    }

    public async Task<TournamentDetails> GetAsync(int id)
    {
        return await context.TournamentDetails.FindAsync(id);
    }

    public void Remove(TournamentDetails tournamentDetails)
    {
        throw new NotImplementedException();
    }

    public async void Update(TournamentDetails tournamentDetails)
    {
        context.Entry(tournamentDetails).State = EntityState.Modified;
        
    }
}
