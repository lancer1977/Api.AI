using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using PolyhydraGames.AI.Rest;
using System.Runtime.InteropServices.JavaScript;

namespace PolyhydraGames.AI.Test;

[TestFixture]
public class RestServiceTests : BaseTest
{
    private readonly AiRestService _ai;
    private IHost _host;
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }
    public RestServiceTests()
    {
        _host = Fixture.Create(x =>
        {
            x.AddSingleton(Factory);
            x.AddSingleton(HttpService);
            x.AddSingleton<AiRestService>();
            x.AddLogging(i =>
            {
                i.AddDebug();

            });
        });
        _ai = _host.Services.GetService<AiRestService>();
    }

    [SetUp]
    public async Task Setup()
    {

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
        var result = await _ai.Generate(request);
        Console.WriteLine(result.Message);
        Assert.That(result.IsSuccess);

    }

    [Test]
    public async Task GetDefinitions()
    {
        var result = await _ai.GetServerDefinitions();
        foreach (var item in result)
        {
            Console.WriteLine(item.Name);
            Console.WriteLine(item.Name + " Avail: " + item.Available);
        }
        
        Assert.That(result.Any());
    }
}

