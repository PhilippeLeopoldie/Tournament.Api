using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournaments.Shared.Dtos;

public record TournamentUpdateDto : TournamentForManipulationDto
{
    public int Id { get; set; }
}
