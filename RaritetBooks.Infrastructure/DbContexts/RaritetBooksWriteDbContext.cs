using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Infrastructure.Interceptors;

namespace RaritetBooks.Infrastructure.DbContexts;

public class RaritetBooksWriteDbContext: DbContext, IUnitOfWork
{
    private readonly IConfiguration _configuration;
    private readonly CacheInvalidationInterceptor _cacheInvalidationInterceptor;

    public DbSet<UserSeller> Sellers => Set<UserSeller>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<SellerRequest> SellerRequests => Set<SellerRequest>();
    public DbSet<User> Users => Set<User>();

    public RaritetBooksWriteDbContext(
        IConfiguration configuration,
        CacheInvalidationInterceptor cacheInvalidationInterceptor)
    {
        _configuration = configuration;
        _cacheInvalidationInterceptor = cacheInvalidationInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("RaritetBooks"));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.AddInterceptors(_cacheInvalidationInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RaritetBooksWriteDbContext).Assembly,
            type => type.FullName?.Contains("Configuration.Write") ?? false);
    }
}