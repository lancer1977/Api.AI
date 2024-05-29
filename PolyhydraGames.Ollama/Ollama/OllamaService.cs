using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Ollama;

public class OllamaService : IAIService
{
    private readonly IOllamaConfig _config;
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        _config = config;
        _client = clientFactory.CreateClient();
    }
    public OllamaService(HttpClient client, IOllamaConfig config)
    {
        _config = config;
        _client = client;
    }
    public async Task<string> GetResponseAsync(string userInput)
    {

        var payload = new
        {
            model = _config.Key,
            prompt = $"{_config.Background}\n\nUser: {userInput}\nDJ Spotabot:",
            stream = false,
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var endpoint = _config.ApiUrl + "/api/generate";
            Debug.WriteLine(endpoint);
            var response = await _client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response: {responseBody}");
            var responseList = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
            return responseList.Response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }
    public async IAsyncEnumerable<string> GetResponseStream(string userInput)
    {

        var payload = new
        {
            model = _config.Key,
            prompt = $"{_config.Background}\n\nUser: {userInput}\nDJ Spotabot:",
           
            stream = true
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var url = _config.ApiUrl + "/api/generate";
        using var client = new HttpClient();
        // Create the content to send in the POST request
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Make the POST request to the API endpoint
        var response = await client.PostAsync(url, content);

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
