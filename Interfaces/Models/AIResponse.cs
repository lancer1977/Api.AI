namespace PolyhydraGames.AI.Models;

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