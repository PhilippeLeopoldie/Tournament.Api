using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Tournaments.Shared.Dtos;

public record GameCreateDto : GameForManipulationDto
{
    [Required(ErrorMessage = "TournamentId Title is a required field.")]
    [JsonProperty("TournamentId")]
    public int TournamentDetailId { get; set; }
}
