using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class ProductPhotoConfiguration : IEntityTypeConfiguration<PhotoProduct>
{
    public void Configure(EntityTypeBuilder<PhotoProduct> builder)
    {
        builder.ToTable("product_photos");
        builder.HasKey(p => p.Id);
    }
}