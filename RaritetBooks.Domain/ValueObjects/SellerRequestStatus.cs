using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.ValueObjects;

public class SellerRequestStatus : ValueObject
{
    public static readonly SellerRequestStatus Declined = new("DECLINED");
    public static readonly SellerRequestStatus Consideration = new("CONSIDERATION");
    public static readonly SellerRequestStatus Approved = new("APPROVED");

    public string Status { get; }

    private SellerRequestStatus(string status)
    {
        Status = status;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Status;
    }
}