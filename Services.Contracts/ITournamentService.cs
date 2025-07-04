using Domain.Models.Entities;
using Tournaments.Shared.Dtos;

namespace Services.Contracts;

public interface ITournamentService
{
    Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync(
        bool includeGames,
        bool sortByTitle,
        bool trackChanges
        );
    Task<TournamentDto> GetTournamentAsync(
        int id,
        bool includeGames,
        bool trackChanges);

    Task<TournamentDetail> PutTournamentAsync(int id, TournamentUpdateDto dto);

    Task<TournamentUpdateDto> TournamentToPatchAsync(int id);

    Task<bool> SavePatchTournamentAsync(int id, TournamentUpdateDto dto);

    Task<TournamentDto> PostTournamentDetails(TournamentCreateDto dto);

    Task<bool> DeleteTournamentAsync(int id);
}
