using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RaritetBooks.Domain.Common;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace RaritetBooks.API.Common;

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
{
    public IActionResult CreateActionResult(
        ActionExecutingContext context,
        ValidationProblemDetails? validationProblemDetails)
    {
        if (validationProblemDetails is null)
            throw new Exception("ValidationProblemDetails is null");

        List<ErrorInfo> errorInfos = [];
        foreach (var (invalidField, validationErrors) in validationProblemDetails.Errors)
        {
            var errors = validationErrors
                .Select(Error.Deserialize)
                .Select(e => new ErrorInfo(e, invalidField));

            errorInfos.AddRange(errors);
        }

        var envelope = Envelope.Error(errorInfos.ToArray());

        return new BadRequestObjectResult(envelope);
    }
}