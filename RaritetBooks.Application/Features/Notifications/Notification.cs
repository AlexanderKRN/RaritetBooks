namespace RaritetBooks.Application.Features.Notifications;

public record Notification(
    Guid UserId,
    string Subject,
    string Message);