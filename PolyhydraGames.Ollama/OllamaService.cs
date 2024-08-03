using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public class OllamaService : IAIService, ILoadAsync
{
    private readonly IOllamaConfig _config;
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private List<ModelDetail> Models { get; set; }
    private List<string> ModelNames { get; set; }
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        _config = config;
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
    private async Task<HttpResponseMessage> GetGenerateResponse(string prompt, string? model = null)
    {
        var payload = new
        {
            model = model ?? _config.Key,
            prompt,
            //prompt = $"{_config.Background}\n\nUser: {userInput}\nDJ Spotabot:",
            stream = false,
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var endpoint = _config.ApiUrl + "/api/generate";
            Debug.WriteLine(endpoint);
            var response = await _client.PostAsync(endpoint, content);
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }


    public async Task<string> GetResponseAsync(string prompt, string? model = null)
    {
        if (string.IsNullOrEmpty(model) || !ModelNames.Contains(model))
        {
            model = _config.Key;
        }
        var response = await GetGenerateResponse(prompt, model);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response: {responseBody}");
        var responseList = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        return responseList.Response;

    }

    public async IAsyncEnumerable<string> GetResponseStream(string prompt, string? model = null)
    {
        var response = await GetGenerateResponse(prompt, model ?? _config.Key);

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
        var endpoint = _config.ApiUrl + "/api/tags";
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
