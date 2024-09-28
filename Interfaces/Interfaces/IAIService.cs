using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.Interfaces
{
    /// <summary>
    /// For communicating with the AI.WebApi microservice.
    /// </summary>
    public interface IAIService
    {
        Task LoadAsync();
        Task<bool> CheckHealth(); 
        Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType payload);
        Task<AiResponseType> GetResponseAsync(string payload);
        Task<AiResponseType> GetResponseAsync(AiRequestType payload);
        IAsyncEnumerable<string> GetResponseStream(AiRequestType payload);
        Task<List<IPersonality>> GetPersonalities();
        ServerDefinitionType ServerDefinition { get; set; }
    }
}