using Ollama.Ollama;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;


public class ServerSource : IServerSource
{
    public Dictionary<ServerDefinitionType, IAIService> Servers { get; set; }


    public ServerSource(IConfiguration config)
    {

        var section = config.GetSection("OllamaItems");
        var types = section.Get<List<ServerDefinitionType>>();
        LoadAsync(types);
    }

    public void LoadAsync(List<ServerDefinitionType> definitions)
    {

        foreach (var server in definitions)
        {
            Servers.Add(server, GetService(server));
        }
    }

    private static IAIService GetService(ServerDefinitionType server)
    {
        switch (server.Type)
        {
            case "Ollama": return new OllamaServer(server);
            default:
                throw new Exception($"Not recognized {server.Type}");
        }

    }

    public Task LoadAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddOrUpdateServer(ServerDefinitionType server)
    {

        var existing = Servers.Keys.FirstOrDefault(x => x.Name == server.Name);
        if (existing != null)
        {
            Servers.Remove(existing);
        }

        var serverInstance = GetService(server);
     Servers.Add(server,);
        return Task.CompletedTask;
    }
     

}