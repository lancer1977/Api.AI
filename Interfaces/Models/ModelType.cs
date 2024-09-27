namespace PolyhydraGames.AI.Models;
    /// <summary>
    /// Informs the consuming app what the model is and some info about it.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    public record ModelType(string Name, string Description, string Prompt);