using AutoMapper;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Contracts;
using Tournament.Infrastructure.Data;
using Tournament.Infrastructure.Repositories;
using Tournament.Presentation.Controllers;
using Tournament.Services;

namespace Tournament.Tests.TestFixtures;

public class DatabaseFixture
{
    public TournamentApiContext Context { get; }
    public IServiceManager ServiceManager { get; }
    public TournamentsController TournamentsController { get; }
    public DatabaseFixture()
    {
        var mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        }));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<TournamentApiContext>()
            .UseSqlServer(configuration.GetConnectionString("TestConnection")).Options;
        Context = new TournamentApiContext(options);
        var unitOfWork = new UnitOfWork(Context);
        var tournamentService = new TournamentService(unitOfWork, mapper);
        var gameService = new GameService(unitOfWork, mapper); 

        ServiceManager = new ServiceManager(
            new Lazy<ITournamentService>(() => tournamentService),
            new Lazy<IGameService>(() => gameService)
        );
        TournamentsController = new TournamentsController(ServiceManager);
        Context.Database.Migrate();
        SeedData();
        Context.SaveChanges();
    }

    private void SeedData()
    {
        if (!Context.TournamentDetails.Any())
        {
            Context.TournamentDetails.AddRange(
                new List<TournamentDetail>
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
                }
            );
        }
    }
}
