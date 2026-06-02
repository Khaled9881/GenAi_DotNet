using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using System.Threading.Tasks;


namespace Functions
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var credential = new ApiKeyCredential(configuration["GitHubModels:Token"]);

            var client = new ChatClientBuilder(new OpenAIClient(credential, new OpenAIClientOptions
            {
                Endpoint = new Uri("https://models.github.ai/inference")

            }).GetChatClient("openai/gpt-4o-mini").AsIChatClient()).UseFunctionInvocation().Build();


            var chatOptions = new ChatOptions
            {
                Tools = [AIFunctionFactory.Create((string location, string unit) => {

                var temperature = Random.Shared.Next(5, 20);
                var conditions = Random.Shared.Next(0, 1) == 0 ? "sunny" : "rainy";

                return $"The weather is {temperature} degrees C and {conditions}.";
                },
                "get_current_weather",
                "Get the current weather in a given location")]
            };

            List<ChatMessage> chatHistory = [new(ChatRole.System, """
            You are a hiking enthusiast who helps people discover fun hikes in their area. 
            You are upbeat and friendly.
            """)];

            // Weather conversation relevant to the registered function.
            chatHistory.Add(new(ChatRole.User, """
            I live in Istanbul and I'm looking for a moderate intensity hike. 
            What's the current weather like? 
            """));

            Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");

            ChatResponse response = await client.GetResponseAsync(chatHistory, chatOptions);

            chatHistory.Add(new(ChatRole.Assistant, response.Text));

            Console.WriteLine($"{chatHistory.Last().Role} >>> {chatHistory.Last()}");


        }
    }
}
