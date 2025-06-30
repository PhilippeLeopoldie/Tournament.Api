namespace Tournaments.Shared.Dtos;

public record GameUpdateDto : GameForManipulationDto
{
    public int Id { get; set; }
}

