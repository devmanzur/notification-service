using FluentValidation;

namespace OrganizationNotificationPlugin.Utils;

/// <summary>
/// Fluent validation utility class to make validations easy
/// </summary>
public static class RuleValidator
{
    public static void Validate<T, TV>(T instance) where T : class where TV : IValidator<T>, new()
    {
        var validator = new TV();
        var validation = validator.Validate(instance);
        if (!validation.IsValid)
        {
            throw new ArgumentException(validation.Errors.FirstOrDefault()?.ErrorMessage,
                validation.Errors.FirstOrDefault()?.PropertyName);
        }
    }
}