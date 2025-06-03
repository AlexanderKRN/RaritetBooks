using RaritetBooks.Application.Features.Notifications;

namespace RaritetBooks.Application.Providers;

public interface IMessageBus
{
    Task PublishAsync(EmailNotification emailNotification, CancellationToken ct);
}