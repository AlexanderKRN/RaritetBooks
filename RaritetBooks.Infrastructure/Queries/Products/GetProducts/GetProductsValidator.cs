using FluentValidation;

namespace RaritetBooks.Infrastructure.Queries.Products.GetProducts;

public class GetProductsValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThan(0);
        RuleFor(x => x.PageSize).NotNull().GreaterThan(0);
    }
}