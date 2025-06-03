using CSharpFunctionalExtensions;

namespace RaritetBooks.Application.Common;

public interface IQueryHandler<in TQuery, TResponse, TError>
{
    Task<Result<TResponse, TError>> Handle(TQuery query, CancellationToken ct);
}

public interface IQueryHandler<TResponse, TError>
{
    Task<Result<TResponse, TError>> Handle(CancellationToken ct);
}

public interface IQuery;