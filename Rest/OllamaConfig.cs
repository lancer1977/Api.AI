namespace PolyhydraGames.AI.Rest;

public record OllamaConfig(string ApiUrl, string Key, string Background) : IOllamaConfig;