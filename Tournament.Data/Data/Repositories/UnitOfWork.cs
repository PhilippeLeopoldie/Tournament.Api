using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournament.Data.Data.Repositories;

public class UnitOfWork(
    TournamentApiContext context,
    ITournamentRepository tournamentRepository,
    IGameRepository gameRepository
    ) : IUnitOfWork
{

    public ITournamentRepository TournamentRepository => tournamentRepository;

    public IGameRepository GameRepository => gameRepository;

    

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
