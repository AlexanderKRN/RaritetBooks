using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellers
{
    public class GetSellersQueryDecorator : IQueryHandler<GetSellersResponse, Error>
    {
        private readonly IQueryHandler<GetSellersResponse, Error> _handler;
        private readonly ILogger<GetSellersQuery> _logger;

        public GetSellersQueryDecorator(
            IQueryHandler<GetSellersResponse, Error> handler,
            ILogger<GetSellersQuery> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task<Result<GetSellersResponse, Error>> Handle(CancellationToken ct)
        {
            #region
            #endregion

            var sellers = await _handler.Handle(ct);

            #region
            #endregion

            return sellers;
        }
    }
}