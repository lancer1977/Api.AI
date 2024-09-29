namespace PolyhydraGames.AI.Rest;

public interface IAIRestConfig
{
    public string ApiUrl { get; }

    public string Background { get;  }
    public string Key { get;  }
}

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);