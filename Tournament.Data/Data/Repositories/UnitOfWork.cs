using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournament.Data.Data.Repositories;

public class UnitOfWork(
    TournamentApiContext context
    ) : IUnitOfWork
{

    public ITournamentRepository TournamentRepository { get; } = new TournamentRepository(context);

    public IGameRepository GameRepository { get; } = new GameRepository(context);

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
