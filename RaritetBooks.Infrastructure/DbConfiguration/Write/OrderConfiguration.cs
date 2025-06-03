using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.ProductId)
            .IsRequired();
        builder.Property(o => o.SalePrice)
            .IsRequired();
        builder.Property(o => o.CreatedDate)
            .IsRequired();
        builder.Property(o => o.UpdatedDate)
            .IsRequired();
        
        builder.ComplexProperty(s => s.Status, b =>
        {
            b.Property(r => r.Status).HasColumnName("status");
        });
        
    }
}