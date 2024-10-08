﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PolyhydraGames.AI.Models;

public record AiRequestType
{
    public AiRequestType(string prompt, string format = "")
    {
        ModelName = "llama3.1";
        Prompt = prompt;
        Format = format;
        Suffix = "";
    }

    public string Format { get; set; }

    [Description("Modile IE llama3.1")]
    public string ModelName { get; set; }

    [Description("The prompt to generate the response for")]
    public string Prompt { get; set; }

    [Description("the text after the model response")]
    public string Suffix { get; set; }

    [Description("Modelfile override")]
    public string? Template { get; set; }
    [Description("Override for the system")]
    public string? System { get; set; }
    [Description("Chat history")]
    public int? Context { get; set; }

    [Description("additional model parameters listed in the documentation for the Modelfile such as temperature")]
    public string? Options { get; set; }
    [Description("Send the response as a stream")]
    public bool? Stream { get; set; }

    [Description("if true no formatting will be applied to the prompt. You may choose to use the raw parameter if you are specifying a full templated prompt in your request to the API")]
    public bool Raw { get; set; }

    //[Description("controls how long the model will stay loaded into memory following the request (default: 5m)")]
    //public string KeepAlive { get; set; } = "5m";
}
//public record AiRequestType<T> : AiRequestType
//{
//    public AiRequestType(string prompt) : base(prompt)
//    {
//    }
//}