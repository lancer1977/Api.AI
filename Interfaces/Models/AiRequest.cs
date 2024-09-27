namespace PolyhydraGames.AI.Models;

public record AiRequestType(string UserPrompt, string Personality = "", IEnumerable<string> additionalMessages = null);