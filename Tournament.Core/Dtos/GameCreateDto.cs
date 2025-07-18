using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Dtos;

public record GameCreateDto : GameForManipulationDto
{
    [Required(ErrorMessage = "TournamentId Title is a required field.")]
    [JsonProperty("TournamentId")]
    public int TournamentDetailId { get; set; }
}
