namespace PolyhydraGames.Ollama.Host
{
    public interface IOllamaHostApi
    {
        Task<bool> AddMessage(string message);
    }
}