using CryptoAnalyzer.Portfolio.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Portfolio.DAL;

public class PortfolioContext(DbContextOptions<PortfolioContext> options) : DbContext(options)
{
    public DbSet<Coin> Coins { get; set; }
    public DbSet<Holding> Holdings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PortfolioContext).Assembly);
    }
}