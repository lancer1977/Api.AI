using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.Configuration.UserSecrets;
using PolyhydraGames.Ollama.Servers; 

namespace PolyhydraGames.AI.WebApi;


/// <summary>
/// Data is placed under the "config" directory
/// </summary>
public static class DataHelper
{

    private static string RootDirectory => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "config");

    static DataHelper()
    {
        if (Path.Exists(RootDirectory)) return;
        Directory.CreateDirectory(RootDirectory);
    }
    public static string GetFilePath(string name)
    {
        return Path.Join(RootDirectory, name);
    }
    
    public static T GetFile<T>(string filename)
    {
        var completeName = GetFilePath(filename);
        var txt = File.ReadAllText(completeName);
        var value = System.Text.Json.JsonSerializer.Deserialize<T>(txt);
        return value;
    }
 
}
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
    private System.Timers.Timer _healthTimer;
 
    private static async Task<IServerSource> InitializeAsync( IServiceProvider provider)
    {
        var types = DataHelper.GetFile<List<ServerDefinitionType>>("servers.json");  
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
        var source = await InitializeAsync( provider.Services);
        source.HealthCheck();
    }

    public void HealthCheck()
    {
        _logger.LogInformation(" Starting checkhealth:" );
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
        _healthTimer = new System.Timers.Timer();
        _healthTimer.Elapsed += (sender, args) => HealthCheck();
        _healthTimer.AutoReset = true;
        _healthTimer.Interval = 2 * 60 * 1000;
        _healthTimer.Start();
        
    }

    public async Task LoadAsync(List<ServerDefinitionType> definitions)
    {

        foreach (var def in definitions)
        {

            var service = await _clientFactory.GetNewService(def, _loggerFactory.CreateLogger<OllamaService>());
            _servers.Add(def, service);
        }
    }

    public async Task<AiResponseType> GetResponseAsync(AiRequestType request)
    {
        var service = GetService();
        try
        {
            return await service.GetResponseAsync(request);

        }
        catch (Exception ex)
        {
            var key = _servers.Where(x => x.Value == service).Select(x => x.Key).First();
            key.Available = false;
            return new AiResponseType(ex.Message);
        }
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