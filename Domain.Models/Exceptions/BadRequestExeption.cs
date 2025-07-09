
namespace Domain.Models.Exceptions;

public abstract class BadRequestException : Exception
{
    public string Title { get; set; }
    protected BadRequestException(string message, string title = "Bad request") : base(message)
    {
        Title = title;
    }
}

public class GlobalBadRequest : BadRequestException
{
    public GlobalBadRequest(int id) : base($"id: '{id}' do not match id from body")
    {
    }
    public GlobalBadRequest() : base($"No patchDocument")
    {
    }
}