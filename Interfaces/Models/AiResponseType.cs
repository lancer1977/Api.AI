namespace PolyhydraGames.AI.Models;

///// <summary>
///// This houses raw AI responses and also the data that is returned.
///// </summary>
///// <typeparam name="T"></typeparam>
///// <param name="RawMessage"></param>
///// <param name="Data"></param>
//public record AiResponseType<T>(string Message, T? Data) : AiResponseType(Message)
//{
//    public override bool IsSuccess => Data != null;
//}
public record AiResponseType(string? Message)
{
    public bool IsSuccess => string.IsNullOrEmpty(Message);
}
