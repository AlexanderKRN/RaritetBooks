using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Infrastructure.DbConfiguration.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);

        builder.ComplexProperty(u => u.Email, 
            emailBuilder => { emailBuilder.Property(e => e.Value).HasColumnName("email"); });
        
        builder.ComplexProperty(u => u.Role, roleBuilder =>
        {
            roleBuilder.Property(r => r.Name).HasColumnName("role");
            roleBuilder.Property(r => r.Permissions).HasColumnName("permissions");
        });

        builder.Property(u => u.PasswordHash).IsRequired();
        
        builder.HasMany(u => u.Orders).WithOne().IsRequired();
    }
}