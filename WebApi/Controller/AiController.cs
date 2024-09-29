using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using PolyhydraGames.AI.Interfaces;

namespace PolyhydraGames.AI.WebApi.Controller;

[Route("[controller]")]
[ApiController]
public class  ServerController : ControllerBase
{
    private readonly IServerSource _viewerService;

    public ServerController(IServerSource viewerService)
    {
        _viewerService = viewerService;
    }
     
    [HttpGet("[action]")]
    public async Task<IEnumerable<IServer>> Items()
    {
        var result =    _viewerService.Items();
        return result.Cast<IServer>();
    }

 
}