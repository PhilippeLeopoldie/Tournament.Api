namespace Domain.Models.Entities;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Time { get; set; }
    public TournamentDetail TournamentDetail { get; set; }
}