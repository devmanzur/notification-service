using System.Text.Json.Serialization;

namespace OrganizationNotificationPlugin.Models;

public class EnvelopeResponse<T> where T : class
{
    public EnvelopeResponse()
    {
    }

    [JsonPropertyName("body")] 
    public T Body { get; set; } = null!;
    [JsonPropertyName("errors")]
    public Dictionary<string, List<string>>? Errors { get; set; }
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }
}