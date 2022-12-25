using FluentValidation;
using FluentValidation.Results;

namespace OrganizationNotificationService.Utils;

/// <summary>
/// This tackles a vulnerability with the fluent validators that just assumes all objects passed are not null
/// and throws a exception instead
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseFluentValidator<T> : AbstractValidator<T> where T : class
{
    protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
    {
        if (context.InstanceToValidate == null)
        {
            result.Errors.Add(new ValidationFailure("body", "Body is null"));
            return false;
        }
        return base.PreValidate(context, result);
    }
}