using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using System.Collections.ObjectModel;

namespace PolyhydraGames.AI.WebApi
{
    public interface IServerSource
    {
        Task<AiResponseType> GetResponseAsync(AiRequestType request);
        Task LoadAsync(List<ServerDefinitionType> definitions);
        Task AddOrUpdateServer(ServerDefinitionType server);
        IEnumerable<ServerDefinitionType> Definitions();
        IEnumerable<IAIService> Items();
        void HealthCheck();
        IReadOnlyDictionary<ServerDefinitionType, IAIService> ReadOnlyServers { get; }

    }
}