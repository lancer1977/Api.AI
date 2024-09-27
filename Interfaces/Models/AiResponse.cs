using System.Text.Json;

namespace PolyhydraGames.AI.Models;

public   static partial class AiResponse
{
    public static AiResponseType<T> Create<T>(string rawResponse)
    {
        var result = new AiResponseType<T>(rawResponse, JsonSerializer.Deserialize<T>(rawResponse));
        return result;
    }
 


}