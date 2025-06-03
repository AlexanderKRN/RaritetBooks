using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RaritetBooks.Application.Features.Notifications;

namespace RaritetBooks.Infrastructure.Kafka;

public static class DependencyRegistration
{
    public static IServiceCollection AddInfrastructureKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHostedService<NotificationConsumer>();

        services.AddSingleton<IKafkaProducer<Notification>, KafkaProducer<Notification>>();
        services.Configure<KafkaOptions>(configuration.GetSection(KafkaOptions.KAFKA));

        services.AddSingleton<KafkaSerializer<Notification>>();

        return services;
    }
}