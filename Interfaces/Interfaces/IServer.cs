using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.Interfaces;

public interface IServer
{
    Task LoadAsync();
    Task<bool> CheckHealth();
    Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType request); 
    IAsyncEnumerable<string> GetResponseStream(AiRequestType request);
    Task<PersonalityType> GetModels();
    string Type { get; set; }
}