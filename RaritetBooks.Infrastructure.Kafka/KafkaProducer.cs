using Confluent.Kafka;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RaritetBooks.Domain.Common;
using Error = RaritetBooks.Domain.Common.Error;

namespace RaritetBooks.Infrastructure.Kafka;

public class KafkaProducer<T> : IKafkaProducer<T>
{
    private readonly ILogger<T> _logger;
    private readonly IProducer<Null, T> _producer;
    private readonly KafkaOptions _kafkaOptions;

    public KafkaProducer(ILogger<T> logger, IOptions<KafkaOptions> kafkaOptions)
    {
        _logger = logger;
        _kafkaOptions = kafkaOptions.Value;
        _producer = CreateProducer();
    }

    public async Task<Result<bool, Error>> Publish(
        string topic, 
        T message, 
        CancellationToken ct)
    {
        var kafkaMessage = new Message<Null, T>()
        {
            Value = message
        };

        var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage, ct);
        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
        {
            _logger.LogError("Message not persisted {message}", kafkaMessage.Value);
            return ErrorList.Kafka.PersistFail();
        }

        _logger.LogInformation("Message persisted {message}", kafkaMessage.Value);
        return true;
    }

    private IProducer<Null, T> CreateProducer()
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = _kafkaOptions.Host,
            AllowAutoCreateTopics = true,
            ClientId = _kafkaOptions.ClientId,
            MessageSendMaxRetries = 3
        };

        return new ProducerBuilder<Null, T>(config)
            .SetValueSerializer(new KafkaSerializer<T>())
            .Build();
    }
}