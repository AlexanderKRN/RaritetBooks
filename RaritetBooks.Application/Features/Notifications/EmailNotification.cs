using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.Application.Features.Notifications;

public record EmailNotification(
    string Subject,
    string Message,
    Email Email);