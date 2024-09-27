namespace Ollama.Models;

public class Tool
{
    public string type { get; set; }
    public Function function { get; set; }
}