using FluentValidation;
using OrganizationNotificationService.Exceptions;

namespace OrganizationNotificationService.Utils;

public static class RuleValidator
{
    public static void Validate<T, TV>(T instance) where T : class where TV : IValidator<T>, new()
    {
        var validator = new TV();
        var validation = validator.Validate(instance);
        if (!validation.IsValid)
        {
            throw new DomainValidationException("One or more validation checks failed",
                "Validation rules check failed", validation.Errors);
        }
    }
}