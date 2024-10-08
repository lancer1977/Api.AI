﻿
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using PolyhydraGames.AI.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Ollama.Models;
using PolyhydraGames.Ollama.Payloads;

namespace PolyhydraGames.Ollama.Servers;

public class OllamaService : IOllamaService, ILoadAsync
{
    private readonly IOllamaConfig _config;
    private readonly ILogger<OllamaService> _logger;

    //private readonly IOllamaConfig _config;
    private string ApiUrl => _config.ApiUrl;
    private string DefaultModel => _config.DefaultModel;
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private List<ModelDetail>? Models { get; set; }
    private List<string>? ModelNames { get; set; }
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config, ILogger<OllamaService> logger)
    {
        _config = config;
        _logger = logger;
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
            _logger.LogInformation(endpoint);
            var content = GetContent(payload);
            var response = await _client.PostAsync(endpoint, content);
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
            _logger.LogInformation(endpoint);
            var content = GetContent(payload);
            var response = await _client.PostAsync(endpoint, content);
            return response;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }

    public Task<AiResponseType> GetResponseAsync(string prompt)
    {
        var payload = prompt.ToGeneratePayload(DefaultModel);
        return GetResponseAsync(payload);
    }

    //public async Task<AiResponseType> GetResponseAsync(GeneratePayload payload)
    //{
    //    var response = await GetGenerateResponse(payload);
    //      return await response.CreateAsync();
    //}

    public async Task<AiResponseType> GetResponseAsync(GeneratePayload payload)
    {
        var response = await GetGenerateResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Response: {responseBody}");
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        return (ollamaResponse?.Response).Create();
    }

    public async Task<AiResponseType> GetResponseAsync(ChatPayload payload)
    {
        var response = await GetChatResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseList = JsonSerializer.Deserialize<OllamaChatResponse>(responseBody);
        return responseList?.Message.Content.Create() ?? new AiResponseType(string.Empty);

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
        _logger.LogInformation(endpoint);
        var response = await _client.GetAsync(endpoint);
        var raw = await response.Content.ReadAsStringAsync();
        var models = JsonSerializer.Deserialize<ModelResponse>(raw);
        return models ?? throw new NullReferenceException("GetModels returned a null response");
    }
    public async Task<bool> CheckHealth()
    {
        var endpoint = ApiUrl + "";
        var response = await _client.GetAsync(endpoint);
        return response.IsSuccessStatusCode;
    }




    public Task<AiResponseType> GetResponseAsync(IEnumerable<string> payload)
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
