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

            //var prompt = "comapre between any 2 cars and give the output in json format";
            //var response = await client.GetResponseAsync<Car>(prompt);
            //Console.WriteLine(response);

            #endregion


            #region ChtaApp with History

            List<ChatMessage> ChatHistory = new List<ChatMessage>()
            {
                new ChatMessage(ChatRole.System, """
                 You are a friendly hiking enthusiast who helps people discover fun hikes in their area.
                    You introduce yourself when first saying hello.
                    When helping people out, you always ask them for this information
                    to inform the hiking recommendation you provide:
                
                    1. The location where they would like to hike
                    2. What hiking intensity they are looking for
                
                    You will then provide three suggestions for nearby hikes that vary in length
                    after you get that information. You will also share an interesting fact about
                    the local nature on the hikes when making a recommendation. At the end of your
                    response, ask if there is anything else you can help with. 
                """)
            };

            while (true)
            {
                var prompt = Console.ReadLine();
                Console.WriteLine("User >>> " + prompt);
                ChatHistory.Add(new ChatMessage(ChatRole.User, prompt));


                //var answer = await client.GetResponseAsync(ChatHistory);
                //Console.WriteLine(answer);
                //ChatHistory.Add(new ChatMessage(ChatRole.Assistant, answer.Text));

                string res = "";
                await foreach (var item in client.GetStreamingResponseAsync(ChatHistory))
                {
                    Console.Write(item);
                    res += item.Text;
                }

                ChatHistory.Add(new ChatMessage(ChatRole.Assistant, res));



            }


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
