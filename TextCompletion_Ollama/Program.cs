using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace TextCompletion_Ollama
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");

            //while (true)
            //{
            //    var prompt = Console.ReadLine();
            //    //Console.WriteLine(await client.GetResponseAsync(prompt));

            //    await foreach (var item in client.GetStreamingResponseAsync(prompt))
            //    {
            //        Console.Write(item);
            //    }
            //}

            #region Structured Output

            //var prompt = "comapre between any 2 cars and give the output (name color describtion type  price) in json format";
            //var response = await client.GetResponseAsync<Car>(prompt);
            //Console.WriteLine(response);

            #endregion

            #region Structured output

            //var carListings = new[]
            //{
            //    "Check out this stylish 2019 Toyota Camry. It has a clean title, only 40,000 miles on the odometer, and a well-maintained interior. The car offers great fuel efficiency, a spacious trunk, and modern safety features like lane departure alert. Minimum offer price: $18,000. Contact Metro Auto at (555) 111-2222 to schedule a test drive.",
            //    "Lease this sporty 2021 Honda Civic! With only 10,000 miles, it includes a sunroof, premium sound system, and backup camera. Perfect for city driving with its compact size and great fuel mileage. Located in Uptown Motors, monthly lease starts at $250 (excl. taxes). Call (555) 333-4444 for more info.",
            //    "A classic 1968 Ford Mustang, perfect for enthusiasts. The vehicle needs some interior restoration, but the engine runs smoothly. V8 engine, manual transmission, around 80,000 miles. This vintage gem is priced at $25,000. Contact Retro Wheels at (555) 777-8888 if you’re interested.",
            //    "Brand new 2023 Tesla Model 3 for lease. Zero miles, fully electric, autopilot capabilities, and a sleek design. Monthly lease starts at $450. Clean lines, minimalist interior, top-notch performance. For more details, call EVolution Cars at (555) 999-0000.",
            //    "Selling a 2015 Subaru Outback in good condition. 60,000 miles on it, includes all-wheel drive, heated seats, and ample cargo space for family getaways. Minimum offer price: $14,000. Contact Forrest Autos at (555) 222-1212 if you want a reliable adventure companion.",
            //};

            //foreach (var listingText in carListings)
            //{
            //    var response = await client.GetResponseAsync<CarDetails>(
            //        $"""
            //        Convert the following car listing into a JSON object matching this C# schema:
            //        Condition: "New" or "Used"
            //        Make: (car manufacturer)
            //        Model: (car model)
            //        Year: (four-digit year)
            //        ListingType: "Sale" or "Lease"
            //        Price: integer only
            //        Features: array of short strings
            //        TenWordSummary: exactly ten words to summarize this listing

            //        Here is the listing:
            //        {listingText}
            //        """);

            //    if (response.TryGetResult(out var info))
            //    {
            //        // Convert the CarDetails object to JSON for display
            //        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
            //            info, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            //    }
            //    else
            //    {
            //        Console.WriteLine("Response was not in the expected format.");
            //    }
            //}



            #endregion




        }


        class CarDetails
        {
            public required string Condition { get; set; }  // e.g. "New" or "Used"
            public required string Make { get; set; }
            public required string Model { get; set; }
            public int Year { get; set; }
            public CarListingType ListingType { get; set; }
            public int Price { get; set; }
            public required string[] Features { get; set; }
            public required string TenWordSummary { get; set; }
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        enum CarListingType { Sale, Lease }

        //public class Car
        //{
        //    public string Name { get; set; }
        //    public string Description { get; set; }
        //    public string Type { get; set; }
        //    public int price { get; set; }
        //    public string color { get; set; }
        //}
    }
}
