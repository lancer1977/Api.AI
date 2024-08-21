using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public class Payload
{
    public Payload()
    {

    }
    public Payload(string prompt)
    {
        Prompt = prompt;
    }
    /// <summary>
    /// a duration string (such as "10m" or "24h")
    //a number in seconds(such as 3600)
    //any negative number which will keep the model loaded in memory(e.g. -1 or "-1m")
    //'0' which will unload the model immediately after generating a response
    /// </summary>
    [JsonPropertyName("keep_alive")]
    public string KeepAlive { get; set; }
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
    public void SetDurationFromMinutes(int minutes)
    {
        KeepAlive = $"{minutes}m";
    }

    public void SetDurationFromHours(int hours)
    {
        KeepAlive = $"{hours}h";
    }
}

public class OllamaService : IAIService, ILoadAsync
{
    //private readonly IOllamaConfig _config;
    private string ApiUrl { get; }
    private string DefaultModel { get; }
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private List<ModelDetail> Models { get; set; }
    private List<string> ModelNames { get; set; }
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        DefaultModel = config.Key;
        ApiUrl = config.ApiUrl;
        _client = clientFactory.CreateClient();
    }
    //public OllamaService(HttpClient client, IOllamaConfig config)
    //{
    //    _config = config;
    //    _client = client;
    //}
    public async Task LoadAsync()
    {
        var modelResponse = await GetModels();
        Models = modelResponse.Models;
        ModelNames = Models.Select(x => x.Model).ToList();
    }
    private StringContent GetContent(Payload payload)
    {
        if (string.IsNullOrEmpty(payload.Model) || !ModelNames.Contains(payload.Model))
        {
            payload.Model = DefaultModel;
        }
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        return content;
    }
    private async Task<HttpResponseMessage> GetGenerateResponse(Payload payload)
    {
        try
        {
            var endpoint = ApiUrl + "/api/generate";
            Debug.WriteLine(endpoint);
            var response = await _client.PostAsync(endpoint, GetContent(payload));
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }


    public Task<string> GetResponseAsync(string payload)
    {
        return GetResponseAsync(new Payload(payload));
    }

    public async Task<string> GetResponseAsync(Payload payload)
    { 
        var response = await GetGenerateResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response: {responseBody}");
        var responseList = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        return responseList.Response;

    }

    public async IAsyncEnumerable<string> GetResponseStream(Payload payload)
    { 
        var response = await GetGenerateResponse(payload);

        // Check if the request was successful
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to call API. Status code: {response.StatusCode}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } line)
        {
            var local = JsonSerializer.Deserialize<OllamaResponse>(line, _options);
            yield return local.Response;
        }
    }

    public async Task<ModelResponse> GetModels()
    {
        var endpoint = ApiUrl + "/api/tags";
        var response = await _client.GetAsync(endpoint);
        var raw = await response.Content.ReadAsStringAsync();
        var models = JsonSerializer.Deserialize<ModelResponse>(raw);
        return models ?? throw new NullReferenceException("GetModels returned a null response");
    }


    public async IAsyncEnumerable<T> MakePostRequest<T>(string apiUrl, string postData)
    {
        using var client = new HttpClient();
        // Create the content to send in the POST request
        var content = new StringContent(postData, Encoding.UTF8, "application/json");

        // Make the POST request to the API endpoint
        var response = await client.PostAsync(apiUrl, content);

        // Check if the request was successful
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to call API. Status code: {response.StatusCode}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync() is { } line)
        {
            yield return JsonSerializer.Deserialize<T>(line, _options);
        }
    }
}
