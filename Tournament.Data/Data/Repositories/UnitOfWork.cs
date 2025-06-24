using Tournament.Core.Repositories;

namespace Tournament.Data.Data.Repositories;

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

    public void Dispose()
    {
        _context.Dispose();
    }
}
