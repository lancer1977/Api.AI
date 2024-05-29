using System.Diagnostics;
using PolyhydraGames.Ollama;
using PolyhydraGames.Ollama.Ollama;

namespace Api.Ollama.WinForms
{
    public partial class Form1 : Form
    {
        IOllamaService _service;
        public Form1()
        {
            InitializeComponent();
            var  config = new OllamaConfig()
            {
                ApiUrl = "https://ollama.polyhydragames.com",
                Key = "llama3",
                Name = "llama3"
            };
            _service = new OllamaService(new HttpClient(), config);
        }

        private   void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";

            this.BeginInvoke(async () =>
            { 

                await foreach (var item in _service.GetOllamaListResponse(textBox1.Text))
                {
                    Debug.WriteLine(item.Response);
                    textBox2.Text += item.Response;
                }
            });
        }
    }
}
