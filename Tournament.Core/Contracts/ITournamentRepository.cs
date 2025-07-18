using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Request;


//using Tournament.Core.Entities;

namespace Tournament.Core.Contracts;

public interface ITournamentRepository
{
     Task<PagedList<TournamentDetail>> GetAllAsync(
        TournamentRequestParams requestParams,
        bool sortByTitle,
        bool trackChanges
        );

     Task<TournamentDetail> GetByIdAsync(
         int id,
         bool includeGames,
         bool trackChanges);
     Task<bool> AnyAsync(int id);

     void Create(TournamentDetail tournamentDetails);
     //void Update(TournamentDetail tournamentDetails);
     void Delete(TournamentDetail tournamentDetails);
    Task<TournamentDetail> GetByTitleAsync(string title, bool trackChanges);
}
