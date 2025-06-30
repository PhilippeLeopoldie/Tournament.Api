using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Tournament.Core.Entities;

namespace Domain.Contracts;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(bool sortedByTitle);
    Task<Game> GetAsync(int id, bool trackChanges);
    Task<Game?> GetAsync(string title, bool trackChanges);
    //Task<bool> AnyAsync(int id);
    void Create(Game game);
    //void Update(Game game);
    void Delete(Game game);
}
