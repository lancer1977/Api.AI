using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.RestfulService;

namespace PolyhydraGames.AI.Rest;

public class AiRestService : RestServiceBase, IAIService, ILoadAsync
{
    private readonly JsonSerializerOptions _options;
    private List<PersonalityType> Personalities { get; set; }
    private List<string> PersonalityNames { get; set; }
    public Task<IEnumerable<PersonalityType>> GetModels()
    {
        throw new NotImplementedException();
    }

    public string Type { get; set; }
    protected override string Service { get; } = "AI";
    public AiRestService(IEndpointFactory endpoint, IHttpService httpService) : base(endpoint, httpService)
    {
        _options = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };

    }

    public async Task LoadAsync()
    {
        var modelResponse = await GetPersonalities();
        Personalities = modelResponse.Cast<PersonalityType>().ToList();
        PersonalityNames = Personalities.Select(x => x.Name).ToList();
    }


    public Task<AiResponseType> Generate(AiRequestType payload)
    {
        return Post<AiResponseType, AiRequestType>(payload);
    }
    public Task<AiResponseType> Chat(AiRequestType payload)
    {
        return Post<AiResponseType, AiRequestType>(payload);

    }


    public   Task<AiResponseType> GetResponseAsync(AiRequestType payload)
    {
        return Generate(payload); 
    }

    public IAsyncEnumerable<string> GetResponseStream(AiRequestType request)
    {
        throw new NotImplementedException();
    }


    public Task<List<ServerDefinitionType>> GetServerDefinitions()
    {
        return Get<List<ServerDefinitionType>>("Items");
    }

    public async Task<List<IPersonality>> GetPersonalities()
    {
        var result = await Get<List<PersonalityType>>();
        return result?.Cast<IPersonality>().ToList() ?? throw new NullReferenceException("GetModels returned a null response");
    }
    public async Task<bool> HealthCheck()
    {
        return await Get<bool>();
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
