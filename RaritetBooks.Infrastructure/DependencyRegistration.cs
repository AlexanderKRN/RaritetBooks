using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.SellerBlanks;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Features.Users;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.Dapper;
using RaritetBooks.Infrastructure.DbContexts;
using RaritetBooks.Infrastructure.Interceptors;
using RaritetBooks.Infrastructure.Jobs;
using RaritetBooks.Infrastructure.MessageBuses;
using RaritetBooks.Infrastructure.MessageConsumers;
using RaritetBooks.Infrastructure.Options;
using RaritetBooks.Infrastructure.Providers;
using RaritetBooks.Infrastructure.Queries.Products.GetProducts;
using RaritetBooks.Infrastructure.Queries.Sellers.GetProductsOfSellerById;
using RaritetBooks.Infrastructure.Queries.Sellers.GetSellerByIdWithPhotos;
using RaritetBooks.Infrastructure.Queries.Sellers.GetSellers;
using RaritetBooks.Infrastructure.Repository;
using RaritetBooks.Infrastructure.Services;

namespace RaritetBooks.Infrastructure;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataStorages(configuration)
            .AddRepositories()
            .AddQueries()
            .AddProviders()
            .AddInterceptors()
            .AddHangfire(configuration)
            .AddServices()
            .AddJobs()
            .RegisterOptions(configuration)
            .AddConsumers()
            .AddChannels()
            .AddMessageBuses();

        return services;
    }

    private static IServiceCollection AddDataStorages(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<RaritetBooksWriteDbContext>();
        services.AddScoped<RaritetBooksReadDbContext>();
        services.AddSingleton<SqlConnectionFactory>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddMinio(option =>
        {
            var minioOption = configuration.GetSection(MinioOptions.Minio)
                .Get<MinioOptions>() ?? throw new Exception("Minio configuration not found");

            option.WithEndpoint(minioOption.EndPoint);
            option.WithCredentials(minioOption.AccessKey, minioOption.SecretKey);
            option.WithSSL(false);
        });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<ISellerRequestRepository, SellerRequestRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<
            IQueryHandler<Guid, GetProductsOfSellerByIdResponse, Error>,
            GetProductsOfSellerByIdQuery>();

        services.AddScoped<
            IQueryHandler<GetProductsRequest, GetProductsResponse, Error>,
            GetProductsQuery>();
        services.Decorate<
            IQueryHandler<GetProductsRequest, GetProductsResponse, Error>,
            GetProductsQueryDecorator>();

        services.AddScoped<
            IQueryHandler<GetSellersResponse, Error>,
            GetSellersQuery>();
        services.Decorate<
            IQueryHandler<GetSellersResponse, Error>,
            GetSellersQueryDecorator>();

        services.AddScoped<
            IQueryHandler<GetSellerByIdWithPhotosRequest, GetSellerByIdWithPhotosResponse, Error>,
            GetSellerByIdWithPhotosQuery>();
        services.Decorate<
            IQueryHandler<GetSellerByIdWithPhotosRequest, GetSellerByIdWithPhotosResponse, Error>,
            GetSellerByIdWithPhotosQueryDecorator>();

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IMinioProvider, MinioProvider>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddSingleton<ICacheProvider, CacheProvider>();
        services.AddScoped<IMailProvider, MailProvider>();

        return services;
    }

    private static IServiceCollection AddInterceptors(this IServiceCollection services)
    {
        services.AddScoped<CacheInvalidationInterceptor>();

        return services;
    }

    private static IServiceCollection AddHangfire(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(configuration.GetConnectionString("RaritetBooks"))));

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(20));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }

    private static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddScoped<ISellerImageCleanupJob, SellerImageCleanupJob>();
        services.AddScoped<IProductImageCleanupJob, ProductImageCleanupJob>();

        return services;
    }

    private static IServiceCollection RegisterOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.Mail));

        return services;
    }

    private static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.AddHostedService<EmailNotificationConsumer>();

        return services;
    }

    private static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services.AddSingleton<EmailMessageChannel>();
        return services;
    }

    private static IServiceCollection AddMessageBuses(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBus, EmailMessageBus>();
        return services;
    }
}