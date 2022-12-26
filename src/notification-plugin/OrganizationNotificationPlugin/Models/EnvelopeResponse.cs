namespace OrganizationNotificationPlugin.Models;

public class EnvelopeResponse<T> where T : class
{
    public EnvelopeResponse()
    {
    }

    public T Body { get; set; } = null!;
    public Dictionary<string, List<string>>? Errors { get; set; }
    public bool IsSuccess { get; set; }
}