namespace RaritetBooks.Application.Features.Users.Login;

public record LoginResponse(string AccessToken, Guid UserId, string Role);