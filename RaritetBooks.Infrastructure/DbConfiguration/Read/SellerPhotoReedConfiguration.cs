using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Infrastructure.ReadModels;

namespace RaritetBooks.Infrastructure.DbConfiguration.Read;

public class SellerPhotoReedConfiguration : IEntityTypeConfiguration<SellerPhotoReadModel>
{
    public void Configure(EntityTypeBuilder<SellerPhotoReadModel> builder)
    {
        builder.ToTable("seller_photos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.SellerId).HasColumnName("user_seller_id");
    }
}