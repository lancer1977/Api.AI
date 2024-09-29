namespace PolyhydraGames.AI.Models;


public record AiResponseType(string? Message)
{
    public bool IsSuccess => string.IsNullOrEmpty(Message);
    public int? Context { get; set; }
}