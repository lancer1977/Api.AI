using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller
{
    public interface IServerSource
    {
        Task AddOrUpdateServer(ServerType server);
        Task<IEnumerable<ServerType>> Items();
    }
}