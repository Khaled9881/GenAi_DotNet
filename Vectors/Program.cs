using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Connectors.InMemory;
using OpenAI;
using System.ClientModel;
//using System.Numerics.Tensors;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;



namespace Vectors
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var credential = new ApiKeyCredential(config["GitHubModels:Token"]);

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://models.github.ai/inference")
            };

            IEmbeddingGenerator<string, Embedding<float>> generator = new OpenAIClient(credential, options).GetEmbeddingClient("openai/text-embedding-3-small").AsIEmbeddingGenerator();

            #region
            //var response = await generator.GenerateVectorAsync("Hello World!");
            //Console.WriteLine("Emeeding Length  " + response.Span.Length);

            //foreach (var item in response.Span)
            //{
            //    Console.Write(item);
            //}

            //var catVector = await generator.GenerateVectorAsync("cat");
            //var DogVector = await generator.GenerateVectorAsync("dog");
            //var KittyVector = await generator.GenerateVectorAsync("kitten");

            //Console.WriteLine(TensorPrimitives.CosineSimilarity(catVector.Span, DogVector.Span));
            //Console.WriteLine(TensorPrimitives.CosineSimilarity(catVector.Span, KittyVector.Span));
            //Console.WriteLine(TensorPrimitives.CosineSimilarity(DogVector.Span, KittyVector.Span));
            #endregion


            var vectorStore = new InMemoryVectorStore();

            var movieStore = vectorStore.GetCollection<int, Movie>("movies");

            await movieStore.EnsureCollectionExistsAsync();

            foreach (var movie in MovieData.Movies)
            {
                movie.Vector = await generator.GenerateVectorAsync(movie.Description);

                await movieStore.UpsertAsync(movie);
            }

            var prompt = "i want to see a science fiction movie";
            var promptEmbeeding = await generator.GenerateVectorAsync(prompt);

            var searcResults = movieStore.SearchAsync(promptEmbeeding, top: 2);

            await foreach (var result in searcResults)
            {
                Console.WriteLine($"Title: {result.Record.Title}");
                Console.WriteLine($"Description: {result.Record.Description}");
                Console.WriteLine($"Score: {result.Score}");
                Console.WriteLine();
            }



        }
    }
}
