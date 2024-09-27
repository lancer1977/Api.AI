using System.Text.Json;

namespace PolyhydraGames.AI.Models;

public interface IServer
{
    string Name { get; set; }
    string Description { get; set; }
    List<string> Models { get; set; }
    int Priority { get; set; }
    string Availability { get; set; }
    int Speed { get; set; }
    string Address { get; set; }
}

public   static partial class AiResponse
{
    public static AiResponseType<T> Create<T>(string rawResponse)
    {
        var result = new AiResponseType<T>(rawResponse, JsonSerializer.Deserialize<T>(rawResponse));
        return result;
    }

    public static async Task<AiResponseType<T?>> Create<T>(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        if (ollamaResponse == null)
        {
            return new AiResponseType<T?>(string.Empty, default);
        }
        else
        {
            return Create<T?>(ollamaResponse.Response);
        }
    }


}