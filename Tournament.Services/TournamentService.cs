using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Services.Contracts;
using Tournaments.Shared.Dtos;
using Tournaments.Shared.Request;

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

    public async Task<(IEnumerable<TournamentDto> tournamentsDto, MetaData metaData)> GetAllTournamentsAsync(
        TournamentRequestParams requestParams,
        bool sortByTitle = false,
        bool trackChanges = false)
    {
        var pagedList = await _uow.TournamentRepository.GetAllAsync(requestParams, sortByTitle, trackChanges);
        var tournamentsDto = _mapper.Map<IEnumerable<TournamentDto>>(pagedList.Items);
        return (tournamentsDto, pagedList.MetaData);
    }

    public async Task<TournamentDto> GetTournamentByIdAsync(int id, bool includeGames, bool trackChanges)
    {
        return _mapper.Map<TournamentDto>(
            await _uow.TournamentRepository.GetByIdAsync(id, includeGames, trackChanges: false)
            );
    }

    public async Task<TournamentDto> GetTournamentByTitleAsync(string title)
    {
        var tournament = await _uow.TournamentRepository.GetByTitleAsync(title, trackChanges: false);
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task<TournamentDetail> PutTournamentAsync(int id, TournamentUpdateDto dto)
    {
        var tournament = await _uow.TournamentRepository.GetByIdAsync(id, includeGames: false, trackChanges: true);
        if (tournament is null) return tournament;

        var updatedTournament = _mapper.Map(dto, tournament);
        await _uow.CompleteAsync();
        return updatedTournament;
    }

    public async Task<TournamentUpdateDto?> TournamentToPatchAsync(int id)
    {
        var tournamentToPatch = await _uow.TournamentRepository.GetByIdAsync(
            id,
            includeGames: true, 
            trackChanges: true
            );

        return tournamentToPatch is null? null : _mapper.Map<TournamentUpdateDto>(tournamentToPatch);
    }

    public async Task<bool> SavePatchTournamentAsync(int id, TournamentUpdateDto dto)
    {
        var tournamentToPatch = await _uow.TournamentRepository.GetByIdAsync(
            id,
            includeGames: true,
            trackChanges: true
            );
        if (tournamentToPatch is null) return false;
        _mapper.Map(dto, tournamentToPatch);
        await _uow.CompleteAsync();
        return true;
    }

    public async Task<TournamentDto> PostTournamentAsync(TournamentCreateDto dto)
    {
        var tournament = _mapper.Map<TournamentDetail>(dto);
        _uow.TournamentRepository.Create(tournament);
        await _uow.CompleteAsync();
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task<bool> DeleteTournamentAsync(int id)
    {
        var tournament = await _uow.TournamentRepository.GetByIdAsync(
            id,
            includeGames: true,
            trackChanges: true
            );
        if (tournament is null) return false;
        _uow.TournamentRepository.Delete(tournament);
        await _uow.CompleteAsync();
        return true;
    }

    
}
