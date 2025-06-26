using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Tournament.Api.Extentions;
using Tournament.Infrastructure.Data;
using Tournament.Infrastructure.Repositories;

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

        // Add services to the container.

        builder.Services.AddControllers( option => option.ReturnHttpNotAcceptable = true)
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(typeof(TournamentMappings));

        var app = builder.Build();
        
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
