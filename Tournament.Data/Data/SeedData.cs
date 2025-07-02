using Bogus;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tournament.Infrastructure.Data;

public static class SeedData
{
    private static readonly string[] tournamentsTitle = { "UEFA champions league", "BasketBall europa league", "HandBall europa league" };
    private static Queue<string> tournamentQueue = new Queue<string>(tournamentsTitle);
    private static int gameCounter;

    public static async Task SeedDataAsync(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var tournamentContext = serviceProvider.GetRequiredService<TournamentApiContext>();
            await tournamentContext.Database.MigrateAsync();
            if (await tournamentContext.TournamentDetails.AnyAsync())
            {
                return;
            }
            try
            {
                var tournaments = GenerateTournament();
                tournamentContext.AddRange(tournaments);
                await tournamentContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public static List<TournamentDetail> GenerateTournament()
    {
        var faker = new Faker<TournamentDetail>().Rules((fake,Tournament) => 
        {
            Tournament.Title = tournamentQueue.Dequeue();
            // Random future date within the next year, with time set to 00:00:00 (midnight)
            Tournament.StartDate = fake.Date.Future(1).Date;
            Tournament.EndDate = Tournament.StartDate.AddMonths(3);
            Tournament.Games = GenerateGame(Tournament.StartDate, Tournament.EndDate);
        });
        return faker.Generate(tournamentsTitle.Count());
    }

    private static ICollection<Game> GenerateGame(DateTime startDate, DateTime endDate)
    {
        var nbrOfGame = new Random().Next(20, 31);
        gameCounter = 1;
        var gameInterval = (endDate - startDate)/nbrOfGame;
        DateTime currentDate = startDate;
        var faker = new Faker<Game>().Rules((fake, game) => 
        {
            var matchTime = fake.Date.BetweenTimeOnly(new TimeOnly(8, 0), new TimeOnly(17, 0)).ToTimeSpan();
            game.Title = $"Match_{gameCounter++}";
            game.Time = currentDate.Date + matchTime;
            currentDate = currentDate.Add(gameInterval);
        });
        return faker.Generate(nbrOfGame);
    }
}
