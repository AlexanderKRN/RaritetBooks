using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Infrastructure.ReadModels;

namespace RaritetBooks.Infrastructure.DbConfiguration.Read;

public class ProductReadConfiguration : IEntityTypeConfiguration<ProductReadModel>
{
    public void Configure(EntityTypeBuilder<ProductReadModel> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        
        builder
            .HasMany(s => s.Photos)
            .WithOne()
            .HasForeignKey(ph => ph.ProductId)
            .IsRequired();
        
        builder.Property(p => p.SellerId).HasColumnName("user_seller_id");
    }
}