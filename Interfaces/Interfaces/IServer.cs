using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.Interfaces;

public interface IAIService
{
    Task LoadAsync();
    Task<bool> CheckHealth();
    Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType<T> request); 
    IAsyncEnumerable<string> GetResponseStream(AiRequestType request);
    Task<IEnumerable<PersonalityType>> GetModels();

    string Type { get; set; }
}