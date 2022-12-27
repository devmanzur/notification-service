using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrganizationNotificationPlugin.Utils;

internal class ApplicationCustomJsonConverter<T> : JsonConverter<T> where T : class
{

    public override void Write(Utf8JsonWriter writer, T item, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var prop in item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            writer.WriteString(prop.Name, prop.GetValue(item)?.ToString());
        }
        writer.WriteEndObject();
    }
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        //Intentionally not implemented
        throw new NotImplementedException();
    }
}
/// <summary>
/// JSON Serializer used by application to provide single api for serialization and de-serialization concerns
/// </summary>
public static class AppJsonUtils
{
    public static JsonSerializerOptions GetSerializerOptions(){
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        return options;
    }
    
    public static string Serialize<T>(T instance) where T : class
    {
        var options = GetSerializerOptions();
        options.Converters.Add(new ApplicationCustomJsonConverter<T>());
        return JsonSerializer.Serialize(instance, options); ;
    }


    public static T? Deserialize<T>(string payload) where T : class
    {
        var options = GetSerializerOptions();
        options.Converters.Add(new ApplicationCustomJsonConverter<T>());

        return JsonSerializer.Deserialize<T>(payload);
    }

    public static T? Deserialize<T>(byte[] payload) where T : class
    {
        return JsonSerializer.Deserialize<T>(payload);
    }
}