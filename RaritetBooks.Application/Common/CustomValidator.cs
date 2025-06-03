using CSharpFunctionalExtensions;
using FluentValidation;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Common;

public static class CustomValidator
{
    public static IRuleBuilderOptions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom(
            (value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
    }

    public static IRuleBuilderOptions<T, TProperty> NotEmptyWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithError(ErrorList.General.ValueIsRequired());
    }
    
    public static IRuleBuilderOptions<T, string> NotEmailWithError<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .EmailAddress()
            .WithError(ErrorList.General.ValueIsInvalid());
    }
    
    public static IRuleBuilderOptions<T, string> MaximumLengthWithError<T>(
        this IRuleBuilder<T, string> ruleBuilder, int maxLength)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithError(ErrorList.General.InvalidLength());
    }
    
    public static IRuleBuilderOptions<T, TProperty> GreaterThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare, string label)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThan(valueToCompare)
            .WithError(ErrorList.General.InvalidLength(label));
    }

    public static IRuleBuilderOptions<T, TProperty?> GreaterThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder, TProperty valueToCompare, string label)
        where TProperty : struct, IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThan(valueToCompare)
            .WithError(ErrorList.General.InvalidLength(label));
    }

    public static IRuleBuilderOptions<T, TProperty> LessThanWithError<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare, string label)
        where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .LessThan(valueToCompare)
            .WithError(ErrorList.General.InvalidLength(label));
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule
            .WithMessage(error.Serialize());
    }
}