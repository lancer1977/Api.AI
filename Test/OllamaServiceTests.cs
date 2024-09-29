using PolyhydraGames.AI.Models;
using PolyhydraGames.AI.Rest;

namespace PolyhydraGames.AI.Test;

[TestFixture]
public class RestServiceTests
{
    private AiRestService _ai;
    public RestServiceTests()
    {
        var config = new AIRestConfig("http://api.polyhydragames.com/ai", "ollama3.2:latest", "You are DJ Spotabot!");
        _ai = new AiRestService(new HttpClient(), config);

    }

    [Test]
    public async Task CheckHealth()
    {
        var result = await _ai.CheckHealth();
        Assert.That(result);
    }

    public Task<AiResponseType> GetResponseAsync(AiRequestType request)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<string> GetResponseStream(AiRequestType request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PersonalityType>> GetModels()
    {
        throw new NotImplementedException();
    }

    public string Type { get; set; }
}
 
