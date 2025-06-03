using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class SellerConfiguration : IEntityTypeConfiguration<UserSeller>
{
    public void Configure(EntityTypeBuilder<UserSeller> builder)
    {
        builder.ToTable("sellers");

        builder.HasKey(u => u.Id);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<UserSeller>(u => u.Id);

        builder.ComplexProperty(u => u.FullName, b =>
        {
            b.Property(f => f.FirstName).HasColumnName("first_name");
            b.Property(f => f.LastName).HasColumnName("last_name");
            b.Property(f => f.Patronomic).HasColumnName("patronomic").IsRequired(false);
        });

        builder.Property(u => u.Description)
            .IsRequired()
            .HasMaxLength(Constraints.LONG_TITLE_LENGTH);

        builder.ComplexProperty(p => p.PhoneNumber, b =>
        {
            b.Property(a => a.Number)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasMaxLength(Constraints.SHORT_TITLE_LENGTH);
        });

        builder.Property(u => u.RegisteredDate)
            .IsRequired();

        builder.Property(u => u.Rating)
            .IsRequired(false);

        builder.OwnsMany(u => u.SocialContacts, navigationBuilder =>
        {
            navigationBuilder.ToJson();

            navigationBuilder.Property(s => s.Types)
                .HasConversion(
                    s => s.Value,
                    s => SocialTypes.Create(s).Value);
        });

        builder.HasMany(u => u.Photos).WithOne().IsRequired();
        builder.HasMany(u => u.Products).WithOne().IsRequired();
    }
}