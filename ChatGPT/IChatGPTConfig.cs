namespace Api.ChatGPT;

public interface IChatGPTConfig
{
    string ApiKey { get; }
    string ApiUrl { get; }
    string Background { get; }
}