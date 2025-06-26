using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Infrastructure.Data;

public class TournamentApiContext : DbContext
{
    public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
        : base(options)
    {
    }

    public DbSet<TournamentDetail> TournamentDetails { get; set; } = default!;
    public DbSet<Game> Games { get; set; } = default!;
}
