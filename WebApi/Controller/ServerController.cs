using Microsoft.AspNetCore.Mvc;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

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
    public async Task<AiResponseType> Generate(AiRequestType server)
    {
        try
        {

            var src = await _source.GetResponseAsync(server);
            return src;
        }
        catch(Exception ex)
        {
            return  new AiResponseType(ex.Message);
        }
    }

}