using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public interface IAIService
{
    Task<string> GetResponseAsync(string payload);
    Task<string> GetResponseAsync(Payload payload);
    IAsyncEnumerable<string> GetResponseStream(Payload payload);
    Task<ModelResponse> GetModels();
}