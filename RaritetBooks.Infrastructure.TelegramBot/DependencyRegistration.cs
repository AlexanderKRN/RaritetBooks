using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaritetBooks.Infrastructure.TelegramBot;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructureTelegram(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(configuration.GetSection(TelegramOptions.TELEGRAM));
        services.AddHostedService<TelegramWorker>();

        return services;
    }
}