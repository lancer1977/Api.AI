using Ollama.Ollama;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;
 

public class ServerSource : IServerSource
{ 
    public Dictionary<ServerDefinitionType,IAIService> Servers { get; set; }
    

    public ServerSource(IConfiguration config)
    { 

        var section = config.GetSection("OllamaItems");
          var types  = section.Get<List<ServerDefinitionType>>();
          LoadAsync(types);
    }

    public  void LoadAsync(List<ServerDefinitionType> definitions)
    { 
       
        foreach(var server in definitions)
        {
            Servers.Add(server, GetService(server));
        }
    }
    private IAIService GetService(ServerDefinitionType server)
    {
        switch (server.Type)
        {
            case "Ollama": return new OllamaService(server);
            default:
                throw new Exception($"Not recognized {server.Type}");
        }
        
    }

    public   Task AddOrUpdateServer(ServerDefinitionType server)
    {
        var existing = Servers.FirstOrDefault(x => x.ServerDefinition.Name == server.Name);
        if (existing == null)
        {

        }
        else
        {

        }

        existing.
        Servers.Remove(server);}

        return Task.CompletedTask;
    }

    public IEnumerable<ServerDefinitionType> Definitions()
    {
        return Servers.Select(x => x.ServerDefinition);
    }

    public IEnumerable<IAIService> Items()
    {
        return Servers;
    }
}