using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller
{
    public interface IServerSource
    {
        Task LoadAsync();
        Task AddOrUpdateServer(ServerDefinitionType server);
        IEnumerable<ServerDefinitionType> Definitions();
        IEnumerable<IAIService> Items();
    }
}