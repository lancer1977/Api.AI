namespace PolyhydraGames.AI.Models;

/// <summary>
/// Informs the consuming app what the model is and some info about it.
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
public record ModelType(string Name, string Description);

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