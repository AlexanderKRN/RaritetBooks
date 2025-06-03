namespace RaritetBooks.Application.Features.Users.RefreshToken;

public record RefreshTokenResponse(string AccessToken, Guid UserId, string Role);