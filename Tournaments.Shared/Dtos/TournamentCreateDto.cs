using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournaments.Shared.Dtos;

public record TournamentCreateDto:TournamentForManipulationDto
{
    public IEnumerable<GameDto> ? Games { get; set; }
}
