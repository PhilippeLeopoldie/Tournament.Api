using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournaments.Shared.Dtos;

public record GameCreateDto : GameForManipulationDto
{
    [Required(ErrorMessage = "TournamentId Title is a required field.")]
    [DisplayName("TournamentId")]
    public int TournamentDetailId { get; set; }
}
