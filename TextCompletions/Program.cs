using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using System.Security.Cryptography.X509Certificates;
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


            #region Basic Completion
            //while (true)
            //{
            //    var question = Console.ReadLine();
            //    var response = await client.GetResponseAsync(question);
            //    Console.WriteLine(response);
            //}
            #endregion

            #region Streaming

            //while (true)
            //{
            //    var prompt = Console.ReadLine();
            //    Console.WriteLine($"user >>>>> {prompt}");
            //    var answer = client.GetStreamingResponseAsync(prompt);
            //    await foreach (var item in answer)
            //    {
            //        Thread.Sleep(300);
            //        Console.Write(item);
            //    }

            //}


            #endregion

            #region Structured Output

            var prompt = "comapre between any 2 cars and give the output in json format";
            var response = await client.GetResponseAsync<Car>(prompt);
            Console.WriteLine(response);




            #endregion



        }

        public class Car
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public int price { get; set; }
            public string color { get; set; }
        }
    }
}
