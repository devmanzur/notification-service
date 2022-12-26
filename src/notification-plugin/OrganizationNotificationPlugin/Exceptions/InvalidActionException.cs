namespace OrganizationNotificationPlugin.Exceptions;

public class InvalidActionException : BaseApplicationException
{
    public InvalidActionException(string userFriendlyMessage, string errorDetails, int statusCode = 500) : base(
        userFriendlyMessage, new ApplicationException(errorDetails), statusCode)
    {
    }
}