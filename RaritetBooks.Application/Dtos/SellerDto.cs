namespace RaritetBooks.Application.Dtos;

public record SellerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Patronomic,
    IReadOnlyList<string> Photos,
    IReadOnlyList<SocialContactDto> Contacts);