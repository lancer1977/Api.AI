using Microsoft.AspNetCore.Mvc;

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
    public Task<IEnumerable<IViewer>> Items()
    {
        return _viewerService.Items();
    }

    public Task UpdateTwitch()
    {
        throw new NotImplementedException();
    }
}

public interface IServer
{

}


public interface IServerSource
{
    Task AddOrUpdateServer(Server server);
    Task<IEnumerable<IServer>> Items();
}