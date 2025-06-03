using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(Constraints.SHORT_TITLE_LENGTH);
        
        builder.Property(p => p.Author)
            .IsRequired()
            .HasMaxLength(Constraints.SHORT_TITLE_LENGTH);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constraints.LONG_TITLE_LENGTH);
        
        builder.HasMany(p => p.Photos).WithOne().IsRequired();
    }
}