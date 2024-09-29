namespace PolyhydraGames.AI.Rest;

public record AIRestConfig(string ApiUrl, string Key, string Background) : IAIRestConfig;