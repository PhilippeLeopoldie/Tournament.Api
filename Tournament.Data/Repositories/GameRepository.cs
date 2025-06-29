using Domain.Contracts;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class GameRepository : RepositoryBase<Game>, IGameRepository
{
    public GameRepository(TournamentApiContext context) : base(context)
    {
    }
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

    public async Task<Game?> GetAsync(int id, bool trackChanges = false)
    {
       return await FindByCondition(game => game.Id.Equals(id), trackChanges)
            .FirstOrDefaultAsync();
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
