using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class SellerRequestConfiguration : IEntityTypeConfiguration<SellerRequest>
{
    public void Configure(EntityTypeBuilder<SellerRequest> builder)
    {
        builder.ToTable("seller_requests");

        builder.HasKey(s => s.Id);

        builder.ComplexProperty(s => s.FullName, b =>
        {
            b.Property(f => f.FirstName).HasColumnName("first_name");
            b.Property(f => f.LastName).HasColumnName("last_name");
            b.Property(f => f.Patronomic).HasColumnName("patronomic").IsRequired(false);
        });

        builder.ComplexProperty(p => p.PhoneNumber, b =>
        {
            b.Property(a => a.Number)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasMaxLength(Constraints.SHORT_TITLE_LENGTH);
        });

        builder.ComplexProperty(v => v.Email, b =>
        {
            b.Property(v => v.Value).HasColumnName("email");
        });

        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constraints.LONG_TITLE_LENGTH);

        builder.Property(u => u.RegistrationDate)
            .IsRequired();

        builder.ComplexProperty(s => s.Status, b =>
        {
            b.Property(r => r.Status).HasColumnName("status");
        });
    }
}