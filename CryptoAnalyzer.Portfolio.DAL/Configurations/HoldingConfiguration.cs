using CryptoAnalyzer.Portfolio.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CryptoAnalyzer.Portfolio.DAL.Configurations;

public class HoldingConfiguration : IEntityTypeConfiguration<Holding>
{
    public void Configure(EntityTypeBuilder<Holding> builder)
    {
        builder.HasKey(h => h.Id);
        builder.HasIndex(h => h.UserEmail);
        builder.HasOne(h => h.Coin).WithMany();
    }
}