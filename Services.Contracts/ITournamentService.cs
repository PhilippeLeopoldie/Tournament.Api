using Domain.Models.Entities;
using Tournaments.Shared.Dtos;
using Tournaments.Shared.Request;

namespace Services.Contracts;

public interface ITournamentService
{
    Task<(IEnumerable<TournamentDto> tournamentsDto, MetaData metaData)> GetAllTournamentsAsync(
        TournamentRequestParams requestParams,
        bool sortByTitle,
        bool trackChanges
        );
    Task<TournamentDto> GetTournamentByIdAsync(
        int id,
        bool includeGames,
        bool trackChanges);

    Task<TournamentDto> GetTournamentByTitleAsync(string title);

    Task PutTournamentAsync(int id, TournamentUpdateDto dto);

    Task<TournamentUpdateDto> TournamentToPatchAsync(int id);

    Task<bool> SavePatchTournamentAsync(int id, TournamentUpdateDto dto);

    Task<TournamentDto> PostTournamentAsync(TournamentCreateDto dto);

    Task<bool> DeleteTournamentAsync(int id);
}
