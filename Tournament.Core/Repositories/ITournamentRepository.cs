using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories;

public interface ITournamentRepository
{
     Task<IEnumerable<TournamentDetail>> GetAllAsync();
     Task<TournamentDetail> GetAsync(int id);
     Task<bool> AnyAsync(int id);
     void Add(TournamentDetail tournamentDetails);
     void Update(TournamentDetail tournamentDetails);
     void Delete(TournamentDetail tournamentDetails);
}
