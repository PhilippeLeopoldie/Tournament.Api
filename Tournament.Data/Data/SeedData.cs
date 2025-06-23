using Bogus;

using Tournament.Core;

namespace Tournament.Data.Data;

public static class SeedData
{
    public static List<TournamentDetails> GenerateTournament(int nbrTournaments)
    { 
        var faker = new Faker<TournamentDetails>().Rules((fake,Tournament) => 
        {
            string[] tournamentsTitle = { "Football", "BasketBall", "HandBall", "Ice Hockey", "Golf" };
            var nbrOfGame = fake.Random.Int(min: 2, max: 10);
            Tournament.Title = fake.PickRandom(tournamentsTitle);
            Tournament.StartDate = fake.Date.Future(1);
            Tournament.Games = GenerateGame(nbrOfGame, Tournament.StartDate);
        });
        return faker.Generate(nbrTournaments);
    }

    private static ICollection<Game> GenerateGame(int nbrOfGame, DateTime tournamentStartDate)
    {
        
        var faker = new Faker<Game>().Rules((fake, game) => 
        {
            string[] gamesTitle = 
            {
                "SwedishLeague",
                "FrenchLeague", 
                "SpanishLeague", 
                "NorwegianLeague", 
                "FinishLeague" 
            };
            var periodOfTournament = fake.Random.Int(min: 1, max: 365);
            var timeOfGame = fake.Date.BetweenTimeOnly(new TimeOnly(8, 0), new TimeOnly(17, 0)).ToTimeSpan();
            game.Title = fake.PickRandom(gamesTitle);
            game.Time = tournamentStartDate.AddDays(periodOfTournament) + timeOfGame;
        });
        return faker.Generate(nbrOfGame);
    }
}
