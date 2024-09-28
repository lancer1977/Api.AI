using Ollama;
using Ollama.Ollama;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;


public class ServerSource : IServerSource
{
    private readonly IHttpClientFactory _clientFactory;
    public Dictionary<ServerDefinitionType, IAIService> Servers { get;   } = new Dictionary<ServerDefinitionType, IAIService>();


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
            var service = GetService(def);
            Servers.Add(def, service);
        }
    }

    private IAIService GetService(ServerDefinitionType definition)
    {
        switch (definition.Type)
        {
            case "Ollama":
                var srv = new OllamaService(_clientFactory, definition.ToOllamaConfig());
                return new OllamaServer(srv);
            default:
                throw new Exception($"Not recognized {definition.Type}");
        }

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