namespace PolyhydraGames.AI.Models;

/// <summary>
/// This houses raw AI responses and also the data that is returned.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="RawMessage"></param>
/// <param name="Data"></param>
public record AiResponseType<T>(string RawMessage, T? Data)
{
    public bool IsSuccess => Data != null;
}
 