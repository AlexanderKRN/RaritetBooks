using RaritetBooks.Application.Features.Notifications;

namespace RaritetBooks.Application.Providers;

public interface IMailProvider
{
    Task SendMessage(EmailNotification emailNotification);
}