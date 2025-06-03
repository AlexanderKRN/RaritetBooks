using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.SellerBlanks.Apply;

public record ApplySellerBlankRequest(
    string FirstName,
    string LastName,
    string? Patronomic,
    string SellerMobilePhone,
    string Email,
    string Description) : ICommand;