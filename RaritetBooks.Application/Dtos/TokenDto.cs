namespace RaritetBooks.Application.Dtos;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    DateTime? RefreshTokenExpiryTime);