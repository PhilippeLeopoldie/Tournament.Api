using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;

namespace Tournament.Tests.UnitTests;

public class SeedData
{
    public List<TournamentDto> GetTournamentsDto()
    {
        return new List<TournamentDto>()
        {
            new()
            {
                Id = 1,
                Title = "Tournament1",
                StartDate = DateTime.Now,
                Games = []
            },
            new()
            {
                Id = 2,
                Title = "Tournament2",
                StartDate = DateTime.Now,
                Games = []
            },
            new()
            {
                Id = 3,
                Title = "Tournament3",
                StartDate = DateTime.Now,
                Games = []
            }
        };
    }

    public List<TournamentDetail> GetTournaments()
    {
        return new List<TournamentDetail>
        {
            new()
            {
                Id = 1,
                Title = "Tournament1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },
            new()
            {
                Id = 2,
                Title = "Tournament2",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },
            new()
            {
                Id = 3,
                Title = "Tournament3",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },

        };
    }

}
