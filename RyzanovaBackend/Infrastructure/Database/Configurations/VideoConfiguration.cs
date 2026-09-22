using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("videos");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Title).IsRequired().HasMaxLength(200);
        builder.Property(v => v.Url).IsRequired();
        builder.Property(v => v.SequenceNumber).IsRequired();

        builder.HasIndex(v => new { v.ThemeId, v.SequenceNumber }).IsUnique();

        builder.HasOne(v => v.Theme)
               .WithMany(t => t.Videos)
               .HasForeignKey(v => v.ThemeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
