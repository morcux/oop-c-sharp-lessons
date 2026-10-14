using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Phone).IsRequired().HasMaxLength(20);

        builder.Property(p => p.Status)
               .HasConversion<string>()
               .IsRequired()
               .HasDefaultValue(PaymentStatus.Pending);

        builder.HasOne(p => p.Theme)
               .WithMany(t => t.Payments)
               .HasForeignKey(p => p.ThemeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}