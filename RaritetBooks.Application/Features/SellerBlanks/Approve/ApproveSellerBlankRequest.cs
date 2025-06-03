using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.SellerBlanks.Approve
{
    public record ApproveSellerBlankRequest(Guid Id) : ICommand;
}