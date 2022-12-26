namespace OrganizationNotificationService.Exceptions;

/// <summary>
/// This is used as the base exception for any exception class used in the entire application
/// This ensures all exceptions follow the same structure
/// </summary>
public abstract class BaseApplicationException : Exception
{
    /// <summary>
    /// Http status code that indicates the cause the exception
    /// 500 error means something went wrong in our system
    /// 400 error means something is wrong with the provided data and cannot pass validation
    /// </summary>
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