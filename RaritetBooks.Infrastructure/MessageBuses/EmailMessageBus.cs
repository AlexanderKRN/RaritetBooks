using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Providers;

namespace RaritetBooks.Infrastructure.MessageBuses;

public class EmailMessageBus(
    EmailMessageChannel messageChannel,
    ILogger<EmailMessageChannel> logger) : IMessageBus
{
    public async Task PublishAsync(EmailNotification emailNotification, CancellationToken ct)
    {
        await messageChannel.Writer.WriteAsync(emailNotification, ct);
        logger.LogInformation("Email message delivered");
    }
}