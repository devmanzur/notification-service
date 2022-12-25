namespace OrganizationNotificationService.Exceptions;

public class SystemConfigurationMissingException : BaseApplicationException
{
    public SystemConfigurationMissingException(string userFriendlyMessage, Exception actualException,
        int statusCode = 500) : base(userFriendlyMessage, actualException, statusCode)
    {
    }
}