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
    void ITournamentRepository.Add(TournamentDetails tournamentDetails)
    {
        throw new NotImplementedException();
    }

    Task<bool> ITournamentRepository.AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    async Task<IEnumerable<TournamentDetails>> ITournamentRepository.GetAllAsync()
    {
        return await context.TournamentDetails.ToListAsync();
    }

    async Task<TournamentDetails> ITournamentRepository.GetAsync(int id)
    {
        return await context.TournamentDetails.FindAsync(id);
    }

    void ITournamentRepository.Remove(TournamentDetails tournamentDetails)
    {
        throw new NotImplementedException();
    }

    void ITournamentRepository.Update(TournamentDetails tournamentDetails)
    {
        throw new NotImplementedException();
    }
}
