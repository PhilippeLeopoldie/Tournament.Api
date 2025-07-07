using AutoMapper;
using Domain.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournaments.Shared.Dtos;

namespace Tournament.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GameService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<GameDto> GetGameByIdAsync(int id, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GameDto>> GetGameByTitleAsync(bool sortedByTitle)
    {
        throw new NotImplementedException();
    }
}
