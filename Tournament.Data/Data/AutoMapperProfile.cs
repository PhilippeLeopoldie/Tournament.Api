using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;

namespace Tournament.Infrastructure.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<TournamentDetail, TournamentDto>().ReverseMap();
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<TournamentDetail, TournamentUpdateDto>().ReverseMap();
        CreateMap<Game, GameUpdateDto>().ReverseMap();
        CreateMap<TournamentCreateDto, TournamentDetail>();
        CreateMap<GameCreateDto, Game>().ReverseMap();
    }
}
