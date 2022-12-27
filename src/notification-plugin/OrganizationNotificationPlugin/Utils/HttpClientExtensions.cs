using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace OrganizationNotificationPlugin.Utils;
/// <summary>
/// HTTP Client extensions that makes making api calls easy
/// </summary>
public static class HttpClientExtensions
{
    private static string BuildQuery(List<(string Key, string Value)> parameters)
    {
        var sb = new StringBuilder();

        foreach (var parameter in parameters)
        {
            if (sb.Length != 0)
            {
                sb.Append('&');
            }

            sb.Append($"{parameter.Item1}={parameter.Item2}");
        }

        return sb.ToString();
    }

    public static async Task<T> SendPostRequestAsync<T>(this HttpClient httpClient, RequestModel requestModel) where T : class
    {
        httpClient.DefaultRequestHeaders.Clear();
        requestModel.Headers.ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
        var queryParameter = BuildQuery(requestModel.Parameters);

        var response =
            await httpClient.PostAsync($"{requestModel.Url}?{queryParameter}",
                new StringContent(requestModel.JsonBody, Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to parse response");
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Api returned error response, Error: {error}");
    }

    public static async Task<T> SendPutRequestAsync<T>(this HttpClient httpClient, RequestModel requestModel)
    {
        httpClient.DefaultRequestHeaders.Clear();
        requestModel.Headers.ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
        var queryParameter = BuildQuery(requestModel.Parameters);

        var response =
            await httpClient.PutAsync($"{requestModel.Url}?{queryParameter}",
                new StringContent(requestModel.JsonBody, Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to parse response");
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Api returned error response, Error: {error}");
    }

    public static async Task<T> SendEncodedPostRequestAsync<T>(this HttpClient httpClient, RequestModel requestModel)
    {
        httpClient.DefaultRequestHeaders.Clear();
        requestModel.Headers.ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
        // var queryParameter = BuildQuery(request.Parameters);

        var encodedParams = new Dictionary<string, string>();
        requestModel.Parameters.ForEach(x => encodedParams.Add(x.Key, x.Value));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response =
            await httpClient.PostAsync(requestModel.Url,
                new FormUrlEncodedContent(encodedParams));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to parse response");
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new Exception($"Api returned error response, Error: {error}");
    }

    public static async Task<T> SendGetRequestAsync<T>(this HttpClient httpClient, RequestModel requestModel)
    {
        httpClient.DefaultRequestHeaders.Clear();
        requestModel.Headers.ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
        var queryParameter = BuildQuery(requestModel.Parameters);

        var response =
            await httpClient.GetAsync($"{requestModel.Url}?{queryParameter}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to parse response");
        }

        throw new Exception("Api returned error response");
    }

    public static async Task<T> SendDeleteRequestAsync<T>(this HttpClient httpClient, RequestModel requestModel)
    {
        httpClient.DefaultRequestHeaders.Clear();
        requestModel.Headers.ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
        var queryParameter = BuildQuery(requestModel.Parameters);

        var response =
            await httpClient.DeleteAsync($"{requestModel.Url}?{queryParameter}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Failed to parse response");
        }

        throw new Exception("Api returned error response");
    }
}

public class RequestModel
{
    public List<(string Key, string Value)> Headers { get; }
    public List<(string Key, string Value)> Parameters { get; }

    public string JsonBody { get; private set; } = string.Empty;

    public string Url { get; private set; }

    public RequestModel(string url)
    {
        Url = url;
        Headers = new List<(string Key, string Value)>();
        Parameters = new List<(string Key, string Value)>();
    }

    public RequestModel AddHeader(string headerKey, string headerValue)
    {
        Headers.Add(new(headerKey, headerValue));
        return this;
    }

    public RequestModel AddQueryParameter(string paramKey, string paramValue)
    {
        Parameters.Add(new(paramKey, paramValue));
        return this;
    }

    public RequestModel AddJsonBody<T>(T body) where T : class
    {
        JsonBody = AppJsonUtils.Serialize(body);
        return this;
    }
}