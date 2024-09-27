using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public class Tool
{
    public string type { get; set; }
    public Function function { get; set; }
}