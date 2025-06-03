using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Infrastructure.ReadModels;

namespace RaritetBooks.Infrastructure.DbConfiguration.Read;

public class SellerReadConfiguration : IEntityTypeConfiguration<SellerReadModel>
{
    public void Configure(EntityTypeBuilder<SellerReadModel> builder)
    {
        builder.ToTable("sellers");
        builder.HasKey(s => s.Id);
        
        builder
            .HasMany(s => s.Photos)
            .WithOne()
            .HasForeignKey(ph => ph.SellerId)
            .IsRequired();
        
        builder
            .HasMany(s => s.Products)
            .WithOne()
            .HasForeignKey(ph => ph.SellerId)
            .IsRequired();
        
        builder.OwnsMany(s => s.SocialContacts, navigationBuilder =>
        {
            navigationBuilder.ToJson();
        });
    }
}