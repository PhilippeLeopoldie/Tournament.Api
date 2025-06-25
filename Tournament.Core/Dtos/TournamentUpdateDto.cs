using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dtos;

public record TournamentUpdateDto : TounamentForManioulationDto
{
    public int Id { get; set; }
}
