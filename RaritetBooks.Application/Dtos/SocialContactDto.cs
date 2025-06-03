namespace RaritetBooks.Application.Dtos;

public record SocialContactDto
{
    public string Social { get; init; } = string.Empty;
    public string Link { get; init; } = string.Empty;
}