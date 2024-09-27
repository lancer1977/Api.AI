using PolyhydraGames.AI.Interfaces;

namespace PolyhydraGames.AI.Models
{
    public record PersonalityType(string Name, string Description, string Prompt) : IPersonality;
}