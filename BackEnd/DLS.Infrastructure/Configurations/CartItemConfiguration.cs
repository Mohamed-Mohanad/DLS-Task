using DLS.Domain.Constants;
using DLS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLS.Infrastructure.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable(TableNames.CartItems);

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.CartId)
            .IsRequired();

        builder.Property(ci => ci.ProductId)
            .IsRequired();

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ci => ci.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(ci => ci.CreatedOnUtc)
            .IsRequired();

        builder.Property(ci => ci.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}