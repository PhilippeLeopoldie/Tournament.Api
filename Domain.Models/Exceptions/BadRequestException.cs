
namespace Domain.Models.Exceptions;

public abstract class BadRequestException : Exception
{
    public string Title { get; set; }
    protected BadRequestException(string message, string title = "Bad request") : base(message)
    {
        Title = title;
    }
}

public class GlobalBadRequestException : BadRequestException
{
    public GlobalBadRequestException(int id) : base($"id: '{id}' do not match id from body")
    {
    }
    public GlobalBadRequestException() : base($"No patchDocument")
    {
    }
}