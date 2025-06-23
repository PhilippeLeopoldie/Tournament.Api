using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
    public Task<IEnumerable<TournamentDetails>> GetAllAsync();
    public Task<TournamentDetails> GetAsync(int id);
    public Task<bool> AnyAsync(int id);
    public void Add(TournamentDetails tournamentDetails);
    public void Update(TournamentDetails tournamentDetails);
    public void Remove(TournamentDetails tournamentDetails);
}
