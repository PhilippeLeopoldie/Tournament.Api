using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions;

public abstract class NotFoundException: Exception
{
    public string Title { get; set; }

    protected NotFoundException(string message, string title = "Not found"): base(message)
    {
        Title = title;
    }
}

public class TournamentNotFoundException : NotFoundException
{
    public TournamentNotFoundException(int id) : base($"The tournament with id: {id} is not found!")
    {
    }

    public TournamentNotFoundException(string title) : base($"The tournament with title: {title} is not found!")
    {
    }

}



public class GameNotFoundException : NotFoundException
{
    public GameNotFoundException(int id) : base($"The tournament with id: {id} is not found!")
    {
    }

    public GameNotFoundException(string title) : base($"The tournament with title: {title} is not found!")
    {
    }
}
