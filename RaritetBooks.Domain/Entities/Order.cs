using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.ValueObjects;
using Entity = RaritetBooks.Domain.Common.Entity;

namespace RaritetBooks.Domain.Entities;

public class Order : Entity
{
    public Guid ProductId { get; private set; }
    public decimal SalePrice { get; private set; }
    
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }
    
    public OrderStatus Status { get; private set; } = null!;

    private Order()
    {
    }

    private Order(
        Guid productId,
        decimal salePrice,
        DateTime createdDate,
        DateTime? updatedDate,
        OrderStatus status)
    {
        ProductId = productId;
        SalePrice = salePrice;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        Status = status;
    }

    public static Result<Order, Error> Create(
        Guid productId,
        decimal salePrice)
    {
        return new Order(
            productId,
            salePrice,
            DateTime.UtcNow,
            null,
            OrderStatus.Pending);
    }
}