namespace RaritetBooks.Infrastructure.Kafka;

public class KafkaOptions
{
    public const string KAFKA = nameof(Kafka);
    
    public string Host { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string NotificationsTopic { get; set; } = string.Empty;
    public string NotificationsGroupId { get; set; } = string.Empty;
    
    public int NotificationTopicPartitions { get; set; } = 1;
}