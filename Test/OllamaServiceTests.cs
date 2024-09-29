using Microsoft.Extensions.Configuration;
using PolyhydraGames.AI.Models;
using PolyhydraGames.AI.Rest;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Test; public class HttpService : IHttpService
{
    private readonly IConfigurationRoot _configuration;

    public HttpService()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
            .AddUserSecrets("d1921150-b78b-40bf-ba16-9dcf02692536") // Use the UserSecretsId generated earlier
            .Build();
        //var _ownerServiceMoq = new Mock<IIdentityService>();
        var token = _configuration.GetValue("Token", "") ?? "";
        _token = token;
        //_ownerServiceMoq.Setup(p => p.OwnerId).Returns(Guid.Parse(id));
    }
    private string _token;
    public async Task<string> GetAuthToken() => await Task.FromResult(_token);

    public HttpClient GetClient { get; } = new HttpClient();
}
public abstract class BaseTest
{
    //protected string WebApiAddress = "https://api.polyhydragames.com/ai";
    //protected string WebApiAddress = "http://192.168.0.21:285";
    protected readonly string WebApiAddress = "https://localhost:7162";
    protected readonly IEndpointFactory Factory;
    protected readonly IHttpService HttpService;

    public BaseTest()
    {
        var endMock = new Moq.Mock<IEndpointFactory>();
        endMock.Setup(p => p.GetEndpoint()).Returns(WebApiAddress);
        Factory = endMock.Object;
        HttpService = new HttpService();

    }
}
[TestFixture]
public class RestServiceTests : BaseTest
{
    private readonly AiRestService _ai;


    public RestServiceTests()
    {
 
        _ai = new AiRestService(base.Factory, HttpService);

    }

    [Test]
    public async Task CheckHealth()
    {
        var result = await _ai.HealthCheck();
        Assert.That(result);
    }
    [Test]
    public async Task GetResponseAsync()
    {
        var request = new AiRequestType("I am a pretty pony");
        var result = await _ai.GetResponseAsync(request);
        Console.WriteLine(result.Message);
        Assert.That(result.IsSuccess);

    }

 

    public async Task GetDefinitions()
    {
        var items = await _ai.GetPersonalities();
        Assert.That(items.Count > 0);
    }
     
}
 
