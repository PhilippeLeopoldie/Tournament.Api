using System.ComponentModel.DataAnnotations;


namespace Tournaments.Shared.Request;

public class RequestParams
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(2, 100)]
    public int PageSize { get; set; } = 5;
}

public class TournamentRequestParams : RequestParams
{
    public bool IncludeGames { get; set; } = false;
}
