using Bogus;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;
using Tournament.Core.Entities;

namespace Tournament.Data.Data;

public static class SeedData
{
    private static readonly string[] tournamentsTitle = { "UEFA champions league", "BasketBall europa league", "HandBall europa league" };
    private static Queue<string> tournamentQueue = new Queue<string>(tournamentsTitle);
    private static int gameCounter;
    public static List<TournamentDetail> GenerateTournament(int nbrTournaments)
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
