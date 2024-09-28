using Microsoft.AspNetCore.Mvc;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller;
[ApiController]
[Route("api")]
public class ServerController : ControllerBase
{
    public readonly IServerSource _source;

    public ServerController(IServerSource viewerService)
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

}