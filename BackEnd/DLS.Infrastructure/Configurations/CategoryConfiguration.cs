using DLS.Domain.Constants;
using DLS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLS.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableNames.Categories);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(MaxLength.Name)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(MaxLength.Description)
            .IsRequired();

        builder.Property(c => c.ParentId)
            .IsRequired(false);

        builder.Property(c => c.CreatedOnUtc)
            .IsRequired();

        builder.Property(c => c.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}