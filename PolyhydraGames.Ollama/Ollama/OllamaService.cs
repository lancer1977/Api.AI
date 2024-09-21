using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Ollama.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PolyhydraGames.Ollama.Ollama;

public static class AiResponse
{
    public static AiResponseType<T> Create<T>(string rawResponse)
    {
        var result = new AiResponseType<T>(rawResponse, JsonSerializer.Deserialize<T>(rawResponse));
        return result;
    }

    public static async Task<AiResponseType<T?>> Create<T>(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        if (ollamaResponse == null)
        {
            return new AiResponseType<T?>(string.Empty,default(T?));
        }
        else
        {
            return Create<T?>(ollamaResponse.Response);
        }
    }


}
public record AiResponseType<T>(string RawMessage, T? Data)
{
    public bool IsSuccess => Data != null;
}
public class OllamaService : IAIService, ILoadAsync
{
    private readonly IOllamaConfig _config;

    //private readonly IOllamaConfig _config;
    private string ApiUrl => _config.ApiUrl;
    private string DefaultModel => _config.Key;
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private List<ModelDetail> Models { get; set; }
    private List<string> ModelNames { get; set; }
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _config = config;
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        _client = clientFactory.CreateClient();
    }

    public async Task LoadAsync()
    {
        var modelResponse = await GetModels();
        Models = modelResponse.Models;
        ModelNames = Models.Select(x => x.Model).ToList();
    }
    private StringContent GetContent(GeneratePayload payload)
    {
        if (string.IsNullOrEmpty(payload.Model) || !ModelNames.Contains(payload.Model))
        {
            payload.Model = DefaultModel;
        }
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        return content;
    }

    private StringContent GetContent(ChatPayload payload)
    {
        if (string.IsNullOrEmpty(payload.Model) || !ModelNames.Contains(payload.Model))
        {
            payload.Model = DefaultModel;
        }
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        return content;
    }

    private async Task<HttpResponseMessage> GetGenerateResponse(GeneratePayload payload)
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
    public async Task<HttpResponseMessage> GetChatResponse(ChatPayload payload)
    {
        try
        {
            var endpoint = ApiUrl + "/api/chat";
            Debug.WriteLine(endpoint);
            var content = GetContent(payload);
            Debug.WriteLine(await content.ReadAsStringAsync());
            var response = await _client.PostAsync(endpoint, content);
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }

    public Task<string> GetResponseAsync(string prompt)
    {
        var payload = new GeneratePayload()
        {
            Prompt = prompt

        };
        return GetResponseAsync(payload);
    }

    public async Task<AiResponseType<T?>> GetResponseAsync<T>(GeneratePayload payload)
    {
        var response = await GetGenerateResponse(payload);

       
        return await response.Create<T?>();
    }

    public async Task<string> GetResponseAsync(GeneratePayload payload)
    {
        var response = await GetGenerateResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response: {responseBody}");
        var responseList = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        return responseList?.Response ?? "";
    }

    public async Task<string> GetResponseAsync(ChatPayload payload)
    {
        var response = await GetChatResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseList = JsonSerializer.Deserialize<OllamaChatResponse>(responseBody);
        return responseList.Message.Content;

    }

    public async IAsyncEnumerable<string> GetResponseStream(GeneratePayload payload)
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



    public Task<string> GetResponseAsync(IEnumerable<string> payload)
    {
        var chats = payload.Select(x => new ChatMessage()
        {
            Role = x.Contains("dreadbread_bot:")
            ? "admin" : "user",
            Content = x
        }).ToList();
        var chatmodel = new ChatPayload() { Messages = chats };
        return GetResponseAsync(chatmodel);
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
