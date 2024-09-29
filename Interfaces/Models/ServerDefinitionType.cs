using PolyhydraGames.AI.Interfaces;

namespace PolyhydraGames.AI.Models
{
    public class ServerDefinitionType : IServerDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultModel { get; set; }
        public int Priority { get; set; }
        public bool Available { get; set; }
        public int Speed { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
    }
}