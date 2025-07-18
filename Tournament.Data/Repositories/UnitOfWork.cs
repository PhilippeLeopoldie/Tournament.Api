using Tournament.Infrastructure.Data;
using Tournament.Core.Contracts;

namespace Tournament.Infrastructure.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly TournamentApiContext _context;
    public ITournamentRepository TournamentRepository { get; }
    public IGameRepository GameRepository { get; } 

    public UnitOfWork(TournamentApiContext context)
    {
        _context = context;
        TournamentRepository = new TournamentRepository(context);
        GameRepository = new GameRepository(context);
    }

    public async Task CompleteAsync()
    {
         await _context.SaveChangesAsync();
    }

}
