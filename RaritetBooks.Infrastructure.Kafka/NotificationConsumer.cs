using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Providers;

namespace RaritetBooks.Infrastructure.Kafka;

public class NotificationConsumer : BackgroundService
{
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly KafkaOptions _kafkaOptions;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationConsumer(
        ILogger<NotificationConsumer> logger,
        IOptions<KafkaOptions> kafkaOptions,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _kafkaOptions = kafkaOptions.Value;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        try
        {
            using (var consumer = CreateConsumer())
            {
                consumer.Subscribe(_kafkaOptions.NotificationsTopic);
            
                while (!stoppingToken.IsCancellationRequested)
                {
                    var kafkaMessage = consumer.Consume(stoppingToken);
                    if (kafkaMessage is null)
                    {
                        _logger.LogInformation("Message is null");
                        continue;
                    }
                
                    var scope = _scopeFactory.CreateScope();
        
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        
                    await notificationService.Notify(kafkaMessage.Message.Value, stoppingToken);
                    
                    _logger.LogInformation("Message consumed: {message}", kafkaMessage.Message.Value);
        
                    consumer.Commit(kafkaMessage);
                }
            
                consumer.Close();
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while consuming Kafka");
        }
        
        await Task.CompletedTask;
    }
    
    public IConsumer<Ignore, Notification> CreateConsumer()
    {
        var config = new ConsumerConfig()
        {
            BootstrapServers = _kafkaOptions.Host,
            GroupId = _kafkaOptions.NotificationsGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            AllowAutoCreateTopics = true,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = true,
        };

        return new ConsumerBuilder<Ignore, Notification>(config)
            .SetValueDeserializer(new KafkaSerializer<Notification>())
            .Build();
    }
}