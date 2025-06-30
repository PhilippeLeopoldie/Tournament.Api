using Newtonsoft.Json;

namespace Tournaments.Shared.Dtos;

public record GameDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime Time { get; set; }
    [JsonProperty("TournamentId")]
    public int TournamentDetailId { get; set; }
}
