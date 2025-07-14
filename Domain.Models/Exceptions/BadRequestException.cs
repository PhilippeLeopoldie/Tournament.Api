
namespace Domain.Models.Exceptions;

public abstract class BadRequestException : Exception
{
    public string Title { get; set; }
    protected BadRequestException(string message, string title = "Bad request") : base(message)
    {
        Title = title;
    }
}

public class InvalidGameIdForTournamentBadRequestException : BadRequestException
{
    public InvalidGameIdForTournamentBadRequestException(int gameId, int tournamentId ) : base($"there is no gameId: '{gameId}' for tournamentId: '{tournamentId}'")
    {
    }
    
}

public class InvalidEntryBadRequestException : BadRequestException
{
    public InvalidEntryBadRequestException(int id) : base($"Invalid Id: '{id}'")
    {
    }
    public InvalidEntryBadRequestException() : base($"No patchDocument")
    {
    }
}