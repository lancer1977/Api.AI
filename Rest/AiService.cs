using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Rest;

public class AiService : IAIService, ILoadAsync
{
    private readonly IOllamaConfig _config;

    //private readonly IOllamaConfig _config;
    private string ApiUrl => _config.ApiUrl;
    private string DefaultModel => _config.Key;
    readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;
    private List<PersonalityType> Personalities { get; set; }
    private List<string> PersonalityNames { get; set; }
    public AiService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _config = config;
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
        _client = clientFactory.CreateClient();
    }

    public async Task LoadAsync()
    {
        var modelResponse = await GetPersonalities();
        Personalities = modelResponse.Cast<PersonalityType>().ToList();
        PersonalityNames = Personalities.Select(x => x.Name).ToList();
    }
    private StringContent GetContent(AiRequestType payload)
    { 
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        return content;
    }
 
    private async Task<HttpResponseMessage> GetGenerateResponse(AiRequestType payload)
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
    public async Task<HttpResponseMessage> GetChatResponse(AiRequestType payload)
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

    public Task<AiResponseType> GetResponseAsync(string prompt)
    {
        var payload = new AiRequestType(prompt);
        return GetResponseAsync(payload);
    }

    public async Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType payload)
    {
        var response = await GetGenerateResponse(payload);
        return await response.Create<T?>();
    }
 
    public async Task<AiResponseType> GetResponseAsync(AiRequestType payload)
    {
        var response = await GetChatResponse(payload);

        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseList = JsonSerializer.Deserialize<AiResponseType>(responseBody);
        return responseList;

    }

    public async IAsyncEnumerable<string> GetResponseStream(AiRequestType payload)
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
            var local = JsonSerializer.Deserialize<AiResponseType>(line, _options);
            yield return local.Message;
        }
    }

    public async Task<List<IPersonality>> GetPersonalities()
    {
        var endpoint = ApiUrl + "/api/tags";
        var response = await _client.GetAsync(endpoint);
        var raw = await response.Content.ReadAsStringAsync();
        var model = JsonSerializer.Deserialize<List<PersonalityType>>(raw);
        return model?.Cast<IPersonality>().ToList() ?? throw new NullReferenceException("GetModels returned a null response");
    }
    public async Task<bool> CheckHealth()
    {
        var endpoint = ApiUrl + "";
        var response = await _client.GetAsync(endpoint);
        return response.IsSuccessStatusCode;
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
