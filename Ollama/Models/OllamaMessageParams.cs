namespace Ollama.Models;

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