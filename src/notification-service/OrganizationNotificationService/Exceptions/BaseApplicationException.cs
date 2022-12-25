namespace OrganizationNotificationService.Exceptions;

/// <summary>
/// This is used as the base exception for any exception class used in the entire application
/// This ensures all exceptions follow the same structure
/// </summary>
public abstract class BaseApplicationException : Exception
{

    public int StatusCode { get; private set; }
    /// <summary>
    /// Only use this to show error to user, the default exception message will be used to 
    /// </summary>
    public string? UserFriendlyMessage { get; private set; }

    public BaseApplicationException(string userFriendlyMessage, Exception actualException, int statusCode = 500) :
        base(actualException.Message, actualException)
    {
        UserFriendlyMessage = userFriendlyMessage;
        StatusCode = statusCode;
    }

    public override string ToString()
    {
        return $"Code: {StatusCode}, Message: {Message}, User Facing Message: {UserFriendlyMessage}";
    }
}