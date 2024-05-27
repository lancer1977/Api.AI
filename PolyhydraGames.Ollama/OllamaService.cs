using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace PolyhydraGames.Ollama;

public class OllamaMessageParams
{    /*
     *arameters
       model: (required) the model name
       messages: the messages of the chat, this can be used to keep a chat memory
       The message object has the following fields:

       role: the role of the message, either system, user or assistant
       content: the content of the message
       images (optional): a list of images to include in the message (for multimodal models such as llava)
       Advanced parameters (optional):

       format: the format to return a response in. Currently the only accepted value is json
       options: additional model parameters listed in the documentation for the Modelfile such as temperature
       stream: if false the response will be returned as a single response object, rather than a stream of objects
       keep_alive: controls how long the model will stay loaded into memory following the request (default: 5m)
     */
    public string model { get; set; }
    public string prompt { get; set; }
    public string message { get; set; }
    public string role { get; set; }
    public string content { get; set; }
    public string[] images { get; set; }
    public string format { get; set; }
    public string options { get; set; }
    public bool stream { get; set; }
    public string keep_alive { get; set; }
    public string messages { get; set; } 


}
public class OllamaService : IOllamaService
{
    private readonly IOllamaConfig _config;
    readonly HttpClient _client;
    public OllamaService(IHttpClientFactory clientFactory, IOllamaConfig config)
    {
        _config = config;
        _client = clientFactory.CreateClient();
    }

    public async Task<OllamaResponse> GetOllamaResponse(string text)
    {

        var payload = new
        {
            model = _config.Key,
            prompt = text,
            stream = false,
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var endpoint = _config.ApiUrl + "/api/generate";
            endpoint = "https://ollama.polyhydragames.com/api/generate";
            Debug.WriteLine(endpoint);
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response: {responseBody}");
            var responseList = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
            return responseList;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }
    public async Task<List<OllamaResponse>> GetOllamaListResponse(string text)
    {
    
        var payload = new
        {
            model = _config.Key,
            prompt = "What is water made of?"
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await _client.PostAsync(_config.ApiUrl, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            
            Console.WriteLine($"Response: {responseBody}");
            var responseList = JsonSerializer.Deserialize<List<OllamaResponse>>(responseBody);
            return responseList;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            throw;
        }
    }
}
