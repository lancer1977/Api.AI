namespace PolyhydraGames.Ollama.Models;

public record AiResponseType<T>(string RawMessage, T? Data)
{
    public bool IsSuccess => Data != null;
}