namespace OrganizationNotificationService.Models;
/// <summary>
/// Result object indicating the result of an action/ command
/// </summary>
/// <param name="IsSuccess"></param>
/// <param name="IsFailure"></param>
/// <param name="FailureDescription"></param>
public abstract record Result(bool IsSuccess, bool IsFailure, string FailureDescription)
{
    public static Result Success()
    {
        return new SuccessResult();
    }
    public static Result Failure(string error)
    {
        return new FailureResult(error);
    }
}

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