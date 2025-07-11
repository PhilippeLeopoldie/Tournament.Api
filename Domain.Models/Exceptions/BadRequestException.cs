
namespace Domain.Models.Exceptions;

public abstract class BadRequestException : Exception
{
    public string Title { get; set; }
    protected BadRequestException(string message, string title = "Bad request") : base(message)
    {
        Title = title;
    }
}

public class InvalidIdBadRequestException : BadRequestException
{
    public InvalidIdBadRequestException(int id) : base($"id: '{id}' do not match tournamentId")
    {
    }
    public InvalidIdBadRequestException() : base($"No patchDocument")
    {
    }
}