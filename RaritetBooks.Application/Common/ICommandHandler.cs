using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace RaritetBooks.Application.Common;

public interface ICommandHandler<in TCommand, TResult, TError>
{
    Task<Result<TResult, TError>> Handle(TCommand command, HttpContext context , CancellationToken ct);
}

public interface ICommandHandler<TResult, TError>
{
    Task<Result<TResult, TError>> Handle(HttpContext context , CancellationToken ct);
}

public interface ICommand;