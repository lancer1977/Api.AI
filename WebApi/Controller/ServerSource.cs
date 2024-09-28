using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;

public class ServerSource : IServerSource
{
    private readonly IHttpClientFactory _clientFactory;
    public Dictionary<ServerDefinitionType, IAIService> Servers { get;   } = new Dictionary<ServerDefinitionType, IAIService>();
    private List<ServerDefinitionType> _onCooldown;

    public ServerSource(IConfiguration config, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;

        var section = config.GetSection("OllamaItems");
        var types = section.Get<List<ServerDefinitionType>>();
        LoadAsync(types);
    }

    public void LoadAsync(List<ServerDefinitionType> definitions)
    {

        foreach (var def in definitions)
        {
            var service = _clientFactory.GetNewService(def);
            Servers.Add(def, service);
        }
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