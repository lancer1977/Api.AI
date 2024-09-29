namespace PolyhydraGames.AI.Models;


public record AiResponseType(string? Message)
{
    public bool IsSuccess => !string.IsNullOrEmpty(Message);
    public List<int>? Context { get; set; }
}