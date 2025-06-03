using CSharpFunctionalExtensions;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Providers
{
    public interface INotificationService
    {
        Task<Result<bool, Error>> Notify(Notification notification, CancellationToken ct);
    }
}