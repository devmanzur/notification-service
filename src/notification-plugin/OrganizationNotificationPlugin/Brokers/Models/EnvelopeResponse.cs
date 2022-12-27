using System.Text.Json.Serialization;

namespace OrganizationNotificationPlugin.Brokers.Models;
/// <summary>
/// Base response wrapper from the API, used by the REST API Broker
/// </summary>
/// <typeparam name="T"></typeparam>
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