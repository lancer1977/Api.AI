using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller;

public class ServerSource : IServerSource
{ 
    public List<ServerType> Servers { get; set; }
    public ServerSource(IConfiguration config)
    {
        Servers = new List<ServerType>();

        var section = config.GetSection("OllamaItems");
          Servers = section.Get<List<ServerType>>();

    }
    public   Task AddOrUpdateServer(ServerType server)
    {
        Servers.Remove(server);

        return Task.CompletedTask;
    }
    public async Task<IEnumerable<ServerType>> Items()
    {
        return Servers;
    }
}