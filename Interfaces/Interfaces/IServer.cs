using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.Interfaces;
public interface IServerDefinition
{
    string Name { get; set; }
    string Description { get; set; }
    List<string> Models { get; set; }
    int Priority { get; set; }
    string Availability { get; set; }
    int Speed { get; set; }
    string Address { get; set; }
}
 
public interface IServer
{
    Task LoadAsync();
    Task<bool> CheckHealth();
    Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType request); 
    IAsyncEnumerable<string> GetResponseStream(AiRequestType request);
    Task<PersonalityType> GetModels();
}