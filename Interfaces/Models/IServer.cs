namespace PolyhydraGames.AI.Models
{
    public interface IServer
    {
        string Name { get; set; }
        string Description { get; set; }
        List<string> Models { get; set; }
        int Priority { get; set; }
        string Availability { get; set; }
        int Speed { get; set; }
        string Address { get; set; }
    }
}