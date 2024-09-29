using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.Interfaces;

public interface IAIService
{
    Task LoadAsync();
    Task<bool> HealthCheck();
    
    Task<AiResponseType> GetResponseAsync(AiRequestType request); 
    IAsyncEnumerable<string> GetResponseStream(AiRequestType request);
    Task<IEnumerable<PersonalityType>> GetModels();
    /// <summary>
    /// Is this Ollama or something else
    /// </summary>
    string Type { get; set; }
}