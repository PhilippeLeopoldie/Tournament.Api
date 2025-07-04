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

public class TournamentService : ITournamentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TournamentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync(
        bool includeGames = false,
        bool sortByTitle = false,
        bool trackChanges = false)
    {
        return _mapper.Map<IEnumerable<TournamentDto>>( await _uow.TournamentRepository
            .GetAllAsync(includeGames,sortByTitle,trackChanges));
    }

    public async Task<TournamentDto> GetTournamentAsync(int id, bool includeGames, bool trackChanges)
    {
        return _mapper.Map<TournamentDto>(
            await _uow.TournamentRepository.GetAsync(id, includeGames, trackChanges: false)
            );
    }

    
}
