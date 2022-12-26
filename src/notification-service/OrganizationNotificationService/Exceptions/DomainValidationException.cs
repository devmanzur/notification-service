using FluentValidation.Results;

namespace OrganizationNotificationService.Exceptions;

public class DomainValidationException : BusinessRuleViolationException
{
    public List<ValidationFailure>? Errors { get; }

    
    public DomainValidationException(string userFriendlyMessage, string systemError, List<ValidationFailure>? errors = null) : base(
        userFriendlyMessage, systemError)
    {
        Errors = errors;
    }
}