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

    Task<IEnumerable<TournamentDetails>> ITournamentRepository.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<TournamentDetails> ITournamentRepository.GetAsync(int id)
    {
        throw new NotImplementedException();
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
