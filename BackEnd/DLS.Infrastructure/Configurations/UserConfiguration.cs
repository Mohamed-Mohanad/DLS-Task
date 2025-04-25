using DLS.Domain.Constants;
using DLS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLS.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .HasMaxLength(MaxLength.Username)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasMaxLength(MaxLength.Password)
            .IsRequired();

        builder.Property(u => u.Name)
            .HasMaxLength(MaxLength.Name)
            .IsRequired();

        builder.Property(u => u.Role)
            .IsRequired();

        builder.Property(u => u.CreatedOnUtc)
            .IsRequired();

        builder.Property(u => u.ModifiedOnUtc)
            .IsRequired(false);

        builder.HasIndex(u => u.Username)
            .IsUnique();
    }
}
