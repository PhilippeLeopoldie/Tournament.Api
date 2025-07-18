namespace Tournament.Core.Dtos;

public record TournamentDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate => StartDate.AddMonths(3);
    public IEnumerable<GameDto>? Games { get; set; }
}
