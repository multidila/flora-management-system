using FloraManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FloraManagement.Persistence;

/// <summary>
/// Database context for working with plants
/// </summary>
public class FlowerDbContext : DbContext
{
    public DbSet<Flower> Flowers { get; set; } = null!;

    public FlowerDbContext(DbContextOptions<FlowerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlowerDbContext).Assembly);
    }
}
