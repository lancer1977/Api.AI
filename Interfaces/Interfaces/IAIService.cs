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
        Task<string> GetResponseAsync(string payload);
        Task<string> GetResponseAsync(AiRequestType payload);
        IAsyncEnumerable<string> GetResponseStream(AiRequestType payload);
        Task<IPersonality> GetPersonalities();
    }
}