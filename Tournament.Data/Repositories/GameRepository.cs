using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class GameRepository(TournamentApiContext context) : IGameRepository
{
    void IGameRepository.Add(Game game)
    {
        throw new NotImplementedException();
    }

    Task<bool> IGameRepository.AnyAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Game>> IGameRepository.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<Game> IGameRepository.GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    void IGameRepository.Remove(Game game)
    {
        throw new NotImplementedException();
    }

    void IGameRepository.Update(Game game)
    {
        throw new NotImplementedException();
    }
}
