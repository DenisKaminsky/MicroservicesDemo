using CommandsService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class AppDbContext : DbContext
{ 
    public DbSet<Platform> Platforms { get; set; } = null!;

    public DbSet<Command> Commands { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>()
            .HasMany(x => x.Commands)
            .WithOne(x => x.Platform)
            .HasForeignKey(x => x.PlatformId);
    }
}