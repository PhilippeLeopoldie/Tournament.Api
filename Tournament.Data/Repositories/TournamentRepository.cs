using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class TournamentRepository(TournamentApiContext context) : ITournamentRepository
{
    public void Add(TournamentDetail tournamentDetails)
    {
        context.TournamentDetails.Add(tournamentDetails);
    }

    public async Task<bool>AnyAsync(int id)
    {
        return await context.TournamentDetails.AnyAsync(tournament => tournament.Id == id);
    }

    public async Task<IEnumerable<TournamentDetail>> GetAllAsync()
    {
        return await context.TournamentDetails.ToListAsync();
    }

    public async Task<TournamentDetail> GetAsync(int id)
    {
        return await context.TournamentDetails.FindAsync(id);
    }

    public void Delete(TournamentDetail tournamentDetails)
    {
        context.TournamentDetails.Remove(tournamentDetails);
    }

    public async void Update(TournamentDetail tournamentDetails)
    {
        context.Entry(tournamentDetails).State = EntityState.Modified;
        
    }
}
