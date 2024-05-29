namespace PolyhydraGames.Ollama.Ollama
{
    public interface IAIService
    {
        Task<string> GetResponseAsync(string text);
        IAsyncEnumerable<string> GetResponseStream(string text);
    }
}