namespace PolyhydraGames.Ollama
{
    public interface IOllamaService
    {
        Task<OllamaResponse> GetOllamaResponse(string text);
        Task<List<OllamaResponse>> GetOllamaListResponse(string text);
    }
}