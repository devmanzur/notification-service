using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Models;

/// <summary>
/// All endpoints/ api responses should Ideally use this one as it provides a uniform interface for all responses
/// </summary>
/// <typeparam name="T"></typeparam>
public class Envelope<T>
{
    // ReSharper disable once MemberCanBeProtected.Global
    protected internal Envelope(T body, string? errorMessage)
    {
        Body = body;
        Errors = ErrorFormatterUtils.ToProblemDetailFormattedErrors(errorMessage);
        IsSuccess = errorMessage == null;
    }

    public T Body { get; }
    public Dictionary<string, List<string>>? Errors { get; }
    public bool IsSuccess { get; }
}

public class Envelope : Envelope<string>
{

    private Envelope(string? errorMessage)
        : base((errorMessage == null ? null : "") ?? string.Empty, errorMessage)
    {
    }

    public static Envelope<T> Ok<T>(T result)
    {
        return new Envelope<T>(result, null);
    }

    public static Envelope Ok()
    {
        return new Envelope(null);
    }

    public static Envelope Error(string errorMessage)
    {
        return new Envelope(errorMessage);
    }

    public static Envelope Error(DomainValidationException validationException)
    {
        return new Envelope(validationException.Errors!.ToSerializedErrors());
    }
}