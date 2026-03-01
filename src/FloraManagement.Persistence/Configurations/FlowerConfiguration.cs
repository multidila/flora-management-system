using FloraManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloraManagement.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Flower entity
/// </summary>
public class FlowerConfiguration : IEntityTypeConfiguration<Flower>
{
    public void Configure(EntityTypeBuilder<Flower> builder)
    {
        builder.ToTable("Flowers");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(f => f.Soil)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(f => f.Origin)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(f => f.Multiplying)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(f => f.CreatedAt)
            .IsRequired();
        builder.Property(f => f.UpdatedAt)
            .IsRequired();
        builder.OwnsOne(f => f.VisualParameters, vp =>
        {
            vp.Property(x => x.StemColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("StemColor");
            vp.Property(x => x.LeafColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("LeafColor");
            vp.Property(x => x.AverageSize)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("AverageSize");
        });
        builder.OwnsOne(f => f.GrowingTips, gt =>
        {
            gt.Property(x => x.TemperatureCelsius)
                .IsRequired()
                .HasColumnName("TemperatureCelsius");
            gt.Property(x => x.IsPhotophilous)
                .IsRequired()
                .HasColumnName("IsPhotophilous");
            gt.Property(x => x.WateringPerWeek)
                .IsRequired()
                .HasColumnName("WateringPerWeek");
        });
    }
}
