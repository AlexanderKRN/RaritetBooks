using FluentValidation;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Sellers.UpdateProduct;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Title)
            .NotEmptyWithError()
            .MaximumLengthWithError(Constraints.SHORT_TITLE_LENGTH);

        RuleFor(x => x.Author)
            .NotEmptyWithError()
            .MaximumLengthWithError(Constraints.SHORT_TITLE_LENGTH);

        RuleFor(x => x.Description)
            .NotEmptyWithError()
            .MaximumLengthWithError(Constraints.LONG_TITLE_LENGTH);
    }
}
