using HorseManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HorseManager.Api.Data;

public class HorseDbContext : DbContext
{
    public HorseDbContext(DbContextOptions<HorseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Horse> Horses { get; set; } = null!;
}
