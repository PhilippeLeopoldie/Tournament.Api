using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;

namespace Tournament.Infrastructure.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings() 
    {
        CreateMap<TournamentDetail, TournamentDto>();
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<TournamentUpdateDto, TournamentDetail>();
        CreateMap<TournamentCreateDto, TournamentDetail>();
    }
}
