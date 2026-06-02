using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using System.Threading.Tasks;


namespace TextCompletions
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                 .AddUserSecrets<Program>()
                 .Build();

            var credential = new ApiKeyCredential(config["GitHubModels:Token"]);

            var client = new OpenAIClient(
                credential,
                new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.github.ai/inference")
                })
                .GetChatClient("openai/gpt-4o-mini")
                .AsIChatClient();


            while (true)
            {
                var question = Console.ReadLine();
                var response = await client.GetResponseAsync(question);
                Console.WriteLine(response);
            }


        }
    }
}
