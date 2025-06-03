using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Queries.Products.GetProducts
{
    public class GetProductsQueryDecorator : IQueryHandler<
        GetProductsRequest, GetProductsResponse, Error>
    {
        private readonly IQueryHandler<
            GetProductsRequest, 
            GetProductsResponse, 
            Error> _handler;
        private readonly ILogger<GetProductsQueryDecorator> _logger;

        public GetProductsQueryDecorator(
            IQueryHandler<GetProductsRequest, GetProductsResponse, Error> handler,
            ILogger<GetProductsQueryDecorator> logger)
        {
            _handler = handler;
            _logger = logger;
        }
        public async Task<Result<GetProductsResponse, Error>> Handle(
            GetProductsRequest query, 
            CancellationToken ct)
        {
            #region
            #endregion

            var productDtosResponse = await _handler.Handle(query, ct);

            #region
            #endregion

            return productDtosResponse;
        }
    }
}