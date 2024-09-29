using PolyhydraGames.AI.Models;
using PolyhydraGames.AI.Rest;

namespace PolyhydraGames.AI.Test;

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

