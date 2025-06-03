using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Kafka
{
    public interface IKafkaProducer<T>
    {
        Task<Result<bool, Error>> Publish(string topic, T message, CancellationToken ct);
    }
}