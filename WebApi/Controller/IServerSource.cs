using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller
{
    public interface IServerSource
    {
        Task<AiResponseType> GetResponseAsync(AiRequestType request); 
         void Load(List<ServerDefinitionType> definitions);
        Task AddOrUpdateServer(ServerDefinitionType server);
        IEnumerable<ServerDefinitionType> Definitions();
        IEnumerable<IAIService> Items();
    }
}