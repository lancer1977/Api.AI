using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using PolyhydraGames.Ollama.Servers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolyhydraGames.AI.WebApi;

public class ServerSource : IServerSource
{
    private bool _initialized;
    private readonly IServerSource _viewerService;

    private readonly ILogger<ServerSource> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Dictionary<ServerDefinitionType, IAIService> _servers = new Dictionary<ServerDefinitionType, IAIService>();
    public IReadOnlyDictionary<ServerDefinitionType, IAIService> ReadOnlyServers => _servers;
    private ServerDefinitionType? _lastDefinition;
    private Timer _healthTimer;
    private static async Task<IServerSource> InitializeAsync(IConfiguration config, IServiceProvider provider)
    {

        var section = config.GetSection("OllamaItems");
        var types = section.Get<List<ServerDefinitionType>>();
        foreach (var type in types)
        {
            type.Id = Guid.NewGuid();
        }
        var source = provider.GetRequiredService<IServerSource>();

        await source.LoadAsync(types);
        return source;
    }

    public static async Task InitializeAsync(WebApplication provider)
    {
        var source = await InitializeAsync(provider.Configuration, provider.Services);
        source.HealthCheck();
    }

    public void HealthCheck()
    {
        Task.Run(async () =>
        {
            foreach (var server in ReadOnlyServers)
            {

                try
                {
                    server.Key.Available = await server.Value.HealthCheck();
                    if (server.Key.Available)
                    {
                        await server.Value.LoadAsync();
                    }
                }
                catch (Exception ex)
                {
                    server.Key.Available = false;
                    _logger.LogError(ex, "Exception calling checkhealth for " + server.Key.Name);
                }
                _logger.LogInformation(server.Key.Name + " checkhealth:" + server.Key.Available);



            }
            _logger.LogInformation("Healthy Servers:" + string.Join(", ", ReadOnlyServers.Keys.Where(x => x.Available).Select(x => x.Name)));
        });
    }

    public ServerSource(ILogger<ServerSource> logger, IHttpClientFactory clientFactory, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _loggerFactory = loggerFactory;
        _healthTimer = new Timer(_ => HealthCheck(), null, Timeout.Infinite, Timeout.Infinite);
        _healthTimer.Change(1 * 5 * 1000, Timeout.Infinite);
    }

    public async Task LoadAsync(List<ServerDefinitionType> definitions)
    {

        foreach (var def in definitions)
        {

            var service = await _clientFactory.GetNewService(def, _loggerFactory.CreateLogger<OllamaService>());
            _servers.Add(def, service);
        }
    }

    public Task<AiResponseType> GetResponseAsync(AiRequestType request)
    {
        return GetService().GetResponseAsync(request);
    }

    private IAIService GetService()
    {
        var key = _servers.Keys
            .Where(x => x.Available).OrderBy(x => x != _lastDefinition)
            .ThenBy(x => x.Priority)

            .First();
        _lastDefinition = key;
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

    public IEnumerable<ServerDefinitionType> Definitions() => _servers.Keys;
    public IEnumerable<IAIService> Items() => _servers.Values;
}