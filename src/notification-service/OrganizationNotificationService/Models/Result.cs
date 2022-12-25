namespace OrganizationNotificationService.Models;

public abstract record Result(bool IsSuccess, bool IsFailure, string FailureDescription);

public record SuccessResult() : Result(true, false, null);

public record FailureResult(string ErrorMessage) : Result(false, true, ErrorMessage)
{
    private readonly string _errorMessage = ErrorMessage;

    public string ErrorMessage
    {
        get => (string.IsNullOrEmpty(_errorMessage) || string.IsNullOrWhiteSpace(_errorMessage))
            ? throw new ArgumentNullException(nameof(ErrorMessage), "Error message cannot be null or empty ")
            : _errorMessage;
        init => _errorMessage = value;
    }
}