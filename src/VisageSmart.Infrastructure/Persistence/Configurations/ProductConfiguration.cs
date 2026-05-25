using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.QuantityInStock)
            .HasColumnName("quantity_in_stock");

        builder.Property(x => x.PurchasePrice)
            .HasColumnName("purchase_price")
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.SellingPrice)
            .HasColumnName("selling_price")
            .HasColumnType("numeric(18,2)");

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
