using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
{
    public void Configure(EntityTypeBuilder<Theme> builder)
    {
        builder.ToTable("themes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(255);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(255);
        builder.Property(t => t.Description).IsRequired();
        builder.Property(t => t.Price).IsRequired();
        builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasIndex(t => t.Slug).IsUnique();

        builder.Property(t => t.Type)
               .HasConversion<string>()
               .IsRequired();
    }
}
