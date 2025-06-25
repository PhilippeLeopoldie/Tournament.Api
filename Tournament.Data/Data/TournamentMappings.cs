using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings() 
    {
        CreateMap<TournamentDetail, TournamentDto>();
        CreateMap<Game, GameDto>();
    }
}
