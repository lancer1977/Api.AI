using Microsoft.AspNetCore.Mvc;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller;

public class ServerController : ControllerBase
{
    public readonly IServerSource _source; 

    public ServerController(IServerSource viewerService)
    {
            _source = viewerService;
        }

    [HttpGet("[action]")]
    public Task<IEnumerable<Server>> Items()
    {
            return _source.Items();
        }
     
    // add a new server
    [HttpPost("[action]")]
    public Task AddOrUpdateServer(Server server)
    {
            return _source.AddOrUpdateServer(server);
        }

}