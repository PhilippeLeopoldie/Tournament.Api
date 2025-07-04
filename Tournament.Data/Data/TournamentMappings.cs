using AutoMapper;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournaments.Shared.Dtos;

namespace Tournament.Infrastructure.Data;

public class TournamentMappings : Profile
{
    public TournamentMappings() 
    {
        CreateMap<TournamentDetail, TournamentDto>().ReverseMap();
        CreateMap<Game, GameDto>().ReverseMap();
        CreateMap<TournamentDetail, TournamentUpdateDto>().ReverseMap();
        CreateMap<Game, GameUpdateDto>().ReverseMap();
        CreateMap<TournamentCreateDto, TournamentDetail>();
        CreateMap<GameCreateDto, Game>().ReverseMap();
    }
}
