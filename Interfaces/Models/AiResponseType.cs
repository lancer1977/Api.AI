namespace PolyhydraGames.AI.Models;

///// <summary>
///// This houses raw AI responses and also the data that is returned.
///// </summary>
///// <typeparam name="T"></typeparam>
///// <param name="RawMessage"></param>
///// <param name="Data"></param>
//public record AiResponseType<T>(string Message, T? Data) : AiResponseType(Message)
//{
//    public override bool IsSuccess => Data != null;
//}
public record AiResponseType(string? Message)
{
    public bool IsSuccess => string.IsNullOrEmpty(Message);
}

public static class AIResponse
{
    public static async Task<AiResponseType> CreateAsync(this HttpResponseMessage message)
    {
        if (!message.IsSuccessStatusCode) return new AiResponseType(null);
        var text = await  message.Content.ReadAsStringAsync();
        return new AiResponseType(text);
    }
    public static AiResponseType Create(this string? message)
    {
        return new AiResponseType(message);
    }
}