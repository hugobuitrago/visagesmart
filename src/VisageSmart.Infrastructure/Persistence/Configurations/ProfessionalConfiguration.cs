using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Infrastructure.Persistence.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Rg)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Services)
            .HasConversion(CreateConverter<List<ProfessionalService>>())
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.Availability)
            .HasConversion(CreateConverter<List<ProfessionalAvailabilityDay>>())
            .HasColumnType("jsonb")
            .IsRequired();
    }

    private static ValueConverter<TProperty, string> CreateConverter<TProperty>()
    {
        return new ValueConverter<TProperty, string>(
            value => JsonSerializer.Serialize(value, JsonOptions),
            value => JsonSerializer.Deserialize<TProperty>(value, JsonOptions)!);
    }
}
