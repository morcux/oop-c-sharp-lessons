using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photos");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Url).IsRequired();
        builder.Property(v => v.SequenceNumber).IsRequired();

        builder.HasIndex(v => new { v.ThemeId, v.SequenceNumber }).IsUnique();

        builder.HasOne(v => v.Theme)
               .WithMany(t => t.Photos)
               .HasForeignKey(v => v.ThemeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
