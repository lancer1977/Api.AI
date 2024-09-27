namespace Ollama.Models;

public class Function
{
    public string name { get; set; }
    public string description { get; set; }
    public Parameters parameters { get; set; }
}