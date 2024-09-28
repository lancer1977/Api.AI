using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller
{
    public interface IServerSource
    {
        public IAIService GetService(ServerDefinitionType definition);
         void LoadAsync(List<ServerDefinitionType> definitions);
        Task AddOrUpdateServer(ServerDefinitionType server);
        IEnumerable<ServerDefinitionType> Definitions();
        IEnumerable<IAIService> Items();
    }
}