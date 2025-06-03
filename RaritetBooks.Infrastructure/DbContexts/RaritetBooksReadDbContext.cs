using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RaritetBooks.Infrastructure.ReadModels;

namespace RaritetBooks.Infrastructure.DbContexts;

public class RaritetBooksReadDbContext: DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<SellerReadModel> Sellers => Set<SellerReadModel>();
    public DbSet<ProductReadModel> Products => Set<ProductReadModel>();

    public RaritetBooksReadDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("RaritetBooks"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RaritetBooksReadDbContext).Assembly,
            type => type.FullName?.Contains("DbConfiguration.Read") ?? false);
    }
}