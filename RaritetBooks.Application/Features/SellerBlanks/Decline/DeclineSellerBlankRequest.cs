using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.SellerBlanks.Decline;

public record DeclineSellerBlankRequest(Guid Id, string Comment) : ICommand;