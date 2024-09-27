using Microsoft.AspNetCore.Mvc;
using PolyhydraGames.AI.Interfaces;

namespace PolyhydraGames.AI.WebApi.Controller;

[Route("[controller]")]
[ApiController]
public class AiController : ControllerBase
{
    private readonly IServerSource _viewerService;

    public AiController(IServerSource viewerService)
    {
        _viewerService = viewerService;
    }
     
    [HttpGet("[action]")]
    public async Task<IEnumerable<IServer>> Items()
    {
        var result = await  _viewerService.Items();
        return result.Cast<IServer>();
    }

 
}