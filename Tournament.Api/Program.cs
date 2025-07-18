using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Tournament.Api.Extensions;
using Tournament.Core.Contracts;
using Tournament.Infrastructure.Data;
using Tournament.Infrastructure.Repositories;
using Tournament.Services;

namespace Tournament.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<TournamentApiContext>(options =>
            options.UseSqlServer(builder.Configuration
            .GetConnectionString("TournamentApiContext") ??
            throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IServiceManager, ServiceManager>();
        builder.Services.AddScoped<ITournamentService, TournamentService>();
        builder.Services.AddScoped<IGameService, GameService>();

        builder.Services.AddLazy<ITournamentService>();
        builder.Services.AddLazy<IGameService>();
        // Add services to the container.

        builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
            .AddNewtonsoftJson();
            //.AddXmlDataContractSerializerFormatters();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        var app = builder.Build();

        app.ConfigureExceptionHandler();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.SeedDataAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
