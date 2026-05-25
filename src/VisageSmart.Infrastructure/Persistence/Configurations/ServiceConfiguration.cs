using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Infrastructure.Persistence.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasMaxLength(80)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
