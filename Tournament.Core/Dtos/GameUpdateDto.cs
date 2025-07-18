namespace Tournament.Core.Dtos;

public record GameUpdateDto : GameForManipulationDto
{
    public int Id { get; set; }
}

