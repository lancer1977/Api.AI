using System.Text;
using System.Text.Json;

namespace Api.ChatGPT
{
    public class ChatGPTService
    {
        private readonly IChatGPTConfig _config;
        public ChatGPTService(IChatGPTConfig config)
        {
            _config = config;
        }
        private   async Task<string> GetResponseFromOpenAI(string personaPrompt, string userInput)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiKey}");

                var requestBody = new
                {
                    prompt = $"{personaPrompt}\n\nUser: {userInput}\nDJ Spotabot:",
                    max_tokens = 150,
                    temperature = 0.7,
                    n = 1,
                    stop = (string)null
                };

                var jsonRequestBody = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_config.ApiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic parsedResponse = JsonSerializer.Deserialize<object>(responseBody);

                return parsedResponse.choices[0].text.Trim();
            }
        }
    }
}
