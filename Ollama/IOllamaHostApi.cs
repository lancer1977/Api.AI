using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public interface IOllamaHostApi
{
    Task<bool> Impersonate(AddMessageParams message);
    Task<bool> AddMessage(AddMessageParams message);
}