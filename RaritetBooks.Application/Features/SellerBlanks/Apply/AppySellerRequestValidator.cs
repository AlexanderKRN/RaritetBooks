using FluentValidation;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.Application.Features.SellerBlanks.Apply;

public class AppySellerRequestValidator : AbstractValidator<ApplySellerBlankRequest>
{
    public AppySellerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmptyWithError()
            .MaximumLengthWithError(Constraints.SHORT_TITLE_LENGTH);

        RuleFor(x => x.LastName)
            .NotEmptyWithError()
            .MaximumLengthWithError(Constraints.SHORT_TITLE_LENGTH);

        RuleFor(x => x.Patronomic)!
            .MaximumLengthWithError(Constraints.SHORT_TITLE_LENGTH);
        
        RuleFor(x => x.SellerMobilePhone).MustBeValueObject(MobilePhone.Create);
       
        RuleFor(x => x.Email).MustBeValueObject(Email.Create);

        RuleFor(x => x.Description)
            .MaximumLengthWithError(Constraints.LONG_TITLE_LENGTH);
    }
}