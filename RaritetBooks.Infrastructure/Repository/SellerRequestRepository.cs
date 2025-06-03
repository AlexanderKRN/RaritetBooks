using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RaritetBooks.Application.Features.SellerBlanks;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Repository;

public class SellerRequestRepository : ISellerRequestRepository
{
    private readonly RaritetBooksWriteDbContext _dbContext;

    public SellerRequestRepository(RaritetBooksWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<SellerRequest, Error>> GetById(Guid id, CancellationToken ct)
    {
        var request = await _dbContext.SellerRequests
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken: ct);
    
        if(request is null)
            return ErrorList.General.NotFound(id);
    
        return request;
    }

    public async Task Add(SellerRequest request, CancellationToken ct)
    {
        await _dbContext.SellerRequests.AddAsync(request, ct);
    }
}