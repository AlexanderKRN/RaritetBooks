using RaritetBooks.Application.Common;

namespace RaritetBooks.Infrastructure.Queries.Products.GetProducts;

public record GetProductsRequest(
    string? Search,
    string SortItem,
    string SortOrder,
    int PageNumber = 1,
    int PageSize = 10) : IQuery;