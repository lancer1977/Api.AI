namespace PolyhydraGames.AI.Models;

public record AiRequestType(string UserPrompt, string Personality = "", IEnumerable<string> additionalMessages = null);
public record AiRequestType<T>(string UserPrompt, string Personality = "", IEnumerable<string> additionalMessages = null) : AiRequestType(UserPrompt, Personality, additionalMessages);