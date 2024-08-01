using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public interface IAIService
{
    Task<string> GetResponseAsync(string prompt, string? model = null);
    IAsyncEnumerable<string> GetResponseStream(string prompt, string? model = null);
    Task<ModelResponse> GetModels();
}