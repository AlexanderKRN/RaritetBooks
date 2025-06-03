using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class SellerPhotoConfiguration : IEntityTypeConfiguration<PhotoSeller>
{
    public void Configure(EntityTypeBuilder<PhotoSeller> builder)
    {
        builder.ToTable("seller_photos");
        builder.HasKey(s => s.Id);
    }
}