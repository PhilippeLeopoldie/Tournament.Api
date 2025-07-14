using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
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
        var tournament = await GetTournamentByIdOrThrowExceptionAsync(id, includeGames, trackChanges:false);
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task<TournamentDto> GetTournamentByTitleAsync(string title)
    {
        var tournament = await _uow.TournamentRepository.GetByTitleAsync(title, trackChanges: false);
        if (tournament is null) throw new TournamentNotFoundException(title);
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task PutTournamentAsync(int id, TournamentUpdateDto dto)
    {
        if (id != dto.Id) throw new InvalidEntryBadRequestException(id);
        var tournament = await GetTournamentByIdOrThrowExceptionAsync(id, includeGames:false, trackChanges: true);
        _mapper.Map(dto, tournament);
        await _uow.CompleteAsync();
    }

    public async Task<(TournamentDetail,TournamentUpdateDto)> TournamentToPatchAsync(int id)
    {
        var tournament = await GetTournamentByIdOrThrowExceptionAsync(id, includeGames: true, trackChanges: true);
        var dto = _mapper.Map<TournamentUpdateDto>(tournament);
        return (tournament,dto);
    }

    public async Task SavePatchTournamentAsync(TournamentDetail tournament ,TournamentUpdateDto dto)
    {
        _mapper.Map(dto, tournament);
        await _uow.CompleteAsync();
    }

    public async Task<TournamentDto> PostTournamentAsync(TournamentCreateDto dto)
    {
        var tournament = _mapper.Map<TournamentDetail>(dto);
        _uow.TournamentRepository.Create(tournament);
        await _uow.CompleteAsync();
        return _mapper.Map<TournamentDto>(tournament);
    }

    public async Task DeleteTournamentAsync(int id)
    {
        var tournament = await GetTournamentByIdOrThrowExceptionAsync(id, includeGames: true, trackChanges: true);
        _uow.TournamentRepository.Delete(tournament);
        await _uow.CompleteAsync();
    }

    private async Task<TournamentDetail> GetTournamentByIdOrThrowExceptionAsync(int id, bool includeGames, bool trackChanges)
    {
        var tournament = await _uow.TournamentRepository.GetByIdAsync(id, includeGames, trackChanges);
        if (tournament is null) throw new TournamentNotFoundException(id);
        return tournament;
    }
    
}
