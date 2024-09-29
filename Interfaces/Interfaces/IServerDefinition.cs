namespace PolyhydraGames.AI.Interfaces
{
    public interface IServerDefinition
    {
        string Name { get; set; }
        string Description { get; set; }
        string DefaultModel { get; set; }
        int Priority { get; set; }
        public bool Available { get; set; }
        int Speed { get; set; }
        string Address { get; set; }
    }
}