using Bogus;
using System.Diagnostics.Metrics;
using Tournament.Core;

namespace Tournament.Data.Data;

public static class SeedData
{
    private static readonly string[] tournamentsTitle = { "UEFA champions league", "BasketBall europa league", "HandBall europa league" };
    private static Queue<string> tournamentQueue = new Queue<string>(tournamentsTitle);
    private static int gameCounter;
    public static List<TournamentDetail> GenerateTournament(int nbrTournaments)
    {
        //string[] tournamentsTitle = { "UEFA champions league", "BasketBall europa league", "HandBall europa league" };
        //var tournamentQueue = new Queue<string>(tournamentsTitle);
         
        var faker = new Faker<TournamentDetail>().Rules((fake,Tournament) => 
        {
            gameCounter = 1;
            var nbrOfGame = fake.Random.Int(min: 20, max: 30);
            Tournament.Title = tournamentQueue.Dequeue();
            Tournament.StartDate = fake.Date.Future(1);
            ;
            Tournament.Games = GenerateGame(nbrOfGame, Tournament.StartDate);
        });
        return faker.Generate(tournamentsTitle.Length);
    }

    private static ICollection<Game> GenerateGame(int nbrOfGame, DateTime tournamentStartDate)
    {
        
        var faker = new Faker<Game>().Rules((fake, game) => 
        {
            //string[] gamesTitle = 
            //{
            //    "PSG-Real VS Madrid",
            //    "Barcelone VS Juventus",
            //    "Manchester VS Napoli",
            //    "Lyon VS Olympiakos", 
            //    "Fenerbache VS Milan" 
            //};
            var periodOfTournament = fake.Random.Int(min: 1, max: 365);
            var timeOfGame = fake.Date.BetweenTimeOnly(new TimeOnly(8, 0), new TimeOnly(17, 0)).ToTimeSpan();
            game.Title = $"Match_{gameCounter++}";
            game.Time = tournamentStartDate.AddDays(periodOfTournament) + timeOfGame;
        });
        return faker.Generate(nbrOfGame);
    }
}
