using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;

namespace Tournament.Api.Extentions;

public static class ApplicationBuilderExtensions
{
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
                // var tournaments = SeedData(10);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
