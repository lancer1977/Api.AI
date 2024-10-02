using Microsoft.AspNetCore.Mvc;

namespace PolyhydraGames.AI.WebApi.Controller;
[ApiController]
[Route("[controller]")]
public class AiController : ControllerBase
{
    public readonly IServerSource _source;

    public AiController(IServerSource viewerService)
    {
        _source = viewerService;
    }

    [HttpGet("[action]")]
    public Task<IEnumerable<ServerDefinitionType>> Items()
    {
        return Task.FromResult(_source.Definitions());
    }

    // add a new server
    [HttpPost("[action]")]
    public Task AddOrUpdateServer(ServerDefinitionType server)
    {
        return _source.AddOrUpdateServer(server);
    }

    [HttpPost("[action]")]
    public async Task<AiResponseType> Generate(AiRequestType request)
    {
        return await _source.GetResponseAsync(request);
    
    }

}