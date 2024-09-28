using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi.Controller;

public class ServerSource : IServerSource
{
    private bool _initialized;
    private readonly ILogger<ServerSource> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly Dictionary<ServerDefinitionType, IAIService> _servers = new Dictionary<ServerDefinitionType, IAIService>();
    public IReadOnlyDictionary<ServerDefinitionType, IAIService> ReadOnlyServers => _servers;
    private ServerDefinitionType? _lastDefinition;
    private Timer _healthTimer;
    private static IServerSource Initialize(IConfiguration config, IServiceProvider provider)
    {

        var section = config.GetSection("OllamaItems");
        var types = section.Get<List<ServerDefinitionType>>();
        var source = provider.GetRequiredService<IServerSource>();
        source.Load(types);
        return source;
    }

    public static async Task InitializeAsync(WebApplication provider)
    {
        var source = Initialize(provider.Configuration, provider.Services);

        source.CheckHealth();

    }

    public void CheckHealth()
    {
        Task.Run(async () =>
        {
            foreach (var server in ReadOnlyServers)
            {

                try
                {
                    server.Key.Available = await server.Value.CheckHealth();
                }
                catch (Exception ex)
                {
                    server.Key.Available = false;
                    _logger.LogError(ex, "Exception calling checkhealth for " + server.Key.Name);
                }
                _logger.LogInformation(server.Key.Name + " checkhealth:" + server.Key.Available);



            }
            _logger.LogInformation("Healthy Servers:" +string.Join(", ",ReadOnlyServers.Keys.Where(x=>x.Available).Select(x=>x.Name)));
        });
    }

    public ServerSource(ILogger<ServerSource> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _healthTimer = new Timer(new TimerCallback((x) => CheckHealth()), null, Timeout.Infinite, Timeout.Infinite);
        _healthTimer.Change(1 * 5 * 1000, Timeout.Infinite);
    }

    public void Load(List<ServerDefinitionType> definitions)
    {

        foreach (var def in definitions)
        {
            var service = _clientFactory.GetNewService(def);
            _servers.Add(def, service);
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
        var key = _servers.Keys
            .Where(x => x.Available)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x != _lastDefinition)
            .First();
        return _servers[key];
    }
    public IAIService GetService(ServerDefinitionType definition)
    {
        var key = _servers.FirstOrDefault(x => x.Key.Id == definition.Id);

        return _servers[key.Key];
    }

    public Task AddOrUpdateServer(ServerDefinitionType server)
    {

        var existing = _servers.Keys.FirstOrDefault(x => x.Name == server.Name);
        if (existing != null)
        {
            _servers.Remove(existing);
        }

        var serverInstance = GetService(server);
        _servers.Add(server, serverInstance);
        return Task.CompletedTask;
    }

    public IEnumerable<ServerDefinitionType> Definitions()
    {
        return _servers.Keys;
    }

    public IEnumerable<IAIService> Items()
    {
        return _servers.Values;
    }
}