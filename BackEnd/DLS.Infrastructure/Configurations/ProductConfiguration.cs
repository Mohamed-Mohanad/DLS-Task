using DLS.Domain.Constants;
using DLS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLS.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasMaxLength(MaxLength.Name)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(MaxLength.Description)
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .IsRequired();

        builder.Property(p => p.CreatedOnUtc)
            .IsRequired();

        builder.Property(p => p.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}