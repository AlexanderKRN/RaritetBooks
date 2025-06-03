using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellerByIdWithPhotos
{
    public class GetSellerByIdWithPhotosQueryDecorator : IQueryHandler<
        GetSellerByIdWithPhotosRequest, 
        GetSellerByIdWithPhotosResponse, 
        Error>
    {
        private readonly IQueryHandler<
            GetSellerByIdWithPhotosRequest, 
            GetSellerByIdWithPhotosResponse,
            Error> _handler;
        private readonly ILogger<GetSellerByIdWithPhotosQuery> _logger;

        public GetSellerByIdWithPhotosQueryDecorator(
            IQueryHandler<
                GetSellerByIdWithPhotosRequest, 
                GetSellerByIdWithPhotosResponse, 
                Error> handler,
            ILogger<GetSellerByIdWithPhotosQuery> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task<Result<GetSellerByIdWithPhotosResponse, Error>> Handle(
            GetSellerByIdWithPhotosRequest query, CancellationToken ct)
        {
            #region
            #endregion

            var seller = await _handler.Handle(query, ct);

            #region
            #endregion

            return seller;
        }
    }
}