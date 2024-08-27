namespace PolyhydraGames.Ollama.Models
{
    public class Parameters
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public List<string> required { get; set; }
    }
}