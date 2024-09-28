namespace PolyhydraGames.AI.Models;


public record AiResponseType(string? Message)
{
    public bool IsSuccess => string.IsNullOrEmpty(Message);
}