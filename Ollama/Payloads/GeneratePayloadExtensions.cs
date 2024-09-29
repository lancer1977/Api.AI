using PolyhydraGames.AI.Models;
using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama.Payloads;

public static class GeneratePayloadExtensions
{
    public static AiResponseType ToResponse(this OllamaResponse response)
    {
        return new AiResponseType(response.Response)
        {
            Context = response.Context
        };
    }

    public static GeneratePayload ToGeneratePayload(this AiRequestType request)
    {
        return new GeneratePayload(request.Prompt)
        {
            Stream = request.Stream,
            Model = request.ModelName,
            Options = request.Options,
            Context = request.Context,
            Template = request.Template,
            System = request.System,
            Suffix = request.Suffix,
            Raw = request.Raw

        };
    }


    public static GeneratePayload ToGeneratePayload(this string prompt)
    {
        return new GeneratePayload(prompt);
    }

    public static GeneratePayload WithMinutes(this GeneratePayload payload, int minutes = 10)
    {
        payload.KeepAlive = $"{minutes}m";
        return payload;
    }

    public static GeneratePayload WithHours(this GeneratePayload payload, int hours)
    {
        payload.KeepAlive = $"{hours}m";
        return payload;
    }

}
