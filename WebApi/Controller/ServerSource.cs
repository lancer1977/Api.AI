using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;

public class ServerSource : IServerSource
{
    private readonly IHttpClientFactory _clientFactory;
    public Dictionary<ServerDefinitionType, IAIService> Servers { get; } = new Dictionary<ServerDefinitionType, IAIService>();
    private ServerDefinitionType? _lastDefinition;

    public static void Initialize(IConfiguration config, IServiceProvider provider)
    {

        var section = config.GetSection("OllamaItems");
        var types = section.Get<List<ServerDefinitionType>>();
        var source = provider.GetRequiredService<IServerSource>();
        source.Load(types);
    }

    public static async Task InitializeAsync(WebApplication provider)
    {
        Initialize(provider.Configuration, provider.Services);
    }

    public ServerSource(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;


    }

    public void Load(List<ServerDefinitionType> definitions)
    {

        foreach (var def in definitions)
        {
            var service = _clientFactory.GetNewService(def);
            Servers.Add(def, service);
        }
    }

    public Task<AiResponseType> GetResponseAsync(AiRequestType request)
    {
        return GetService().GetResponseAsync(request);
    }

    /// <summary>
    /// So this is where the sauce occurs, we find the most availiabvle IAIService and use it.
    /// </summary>
    /// <returns></returns>
    private IAIService GetService()
    {
        var key = Servers.Keys
            .Where(x => x.Available)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x != _lastDefinition)
            .First();
        return Servers[key];
    }
    public IAIService GetService(ServerDefinitionType definition)
    {
        var key = Servers.FirstOrDefault(x => x.Key.Id == definition.Id);

        return Servers[key.Key];
    }

    public Task AddOrUpdateServer(ServerDefinitionType server)
    {

        var existing = Servers.Keys.FirstOrDefault(x => x.Name == server.Name);
        if (existing != null)
        {
            Servers.Remove(existing);
        }

        var serverInstance = GetService(server);
        Servers.Add(server, serverInstance);
        return Task.CompletedTask;
    }

    public IEnumerable<ServerDefinitionType> Definitions()
    {
        return Servers.Keys;
    }

    public IEnumerable<IAIService> Items()
    {
        return Servers.Values;
    }
}