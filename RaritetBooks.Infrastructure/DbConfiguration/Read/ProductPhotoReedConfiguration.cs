using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Infrastructure.ReadModels;

namespace RaritetBooks.Infrastructure.DbConfiguration.Read;

public class ProductPhotoReedConfiguration : IEntityTypeConfiguration<ProductPhotoReadModel>
{
    public void Configure(EntityTypeBuilder<ProductPhotoReadModel> builder)
    {
        builder.ToTable("product_photos");
        builder.HasKey(p => p.Id);
    }
}