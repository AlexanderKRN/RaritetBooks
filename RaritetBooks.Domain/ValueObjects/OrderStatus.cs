using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.ValueObjects;

public class OrderStatus : ValueObject
{
    public static readonly OrderStatus Pending = new("PENDING");
    public static readonly OrderStatus Payed = new("PAYED");
    public static readonly OrderStatus Delivered = new("DELIVERED");
    
    public string Status { get; }
    
    private OrderStatus(string status)
    {
        Status = status;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Status;
    }
}